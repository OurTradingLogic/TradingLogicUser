using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository;
using app.Services.Models;
using DbModel = Database.Models;

namespace app.Services;

public class TradingLogicService: ITradingLogicService
{
    private readonly ILogger<TradingLogicService> _logger;
    private readonly ITradingLogicRepository _tradingLogicRepository;
    private readonly ITradingLogicClientService _tradingLogicClientService;

    public TradingLogicService(ILogger<TradingLogicService> logger, ITradingLogicRepository tradingLogicRepository, ITradingLogicClientService tradingLogicClientService)
    {
        _logger = logger;
        _tradingLogicRepository = tradingLogicRepository;
        _tradingLogicClientService = tradingLogicClientService;
    }

    public async Task<StockSignalResponse> StockSignalList()
    {
        IEnumerable<DbModel.Stock> stockList = await _tradingLogicRepository.GetStockList();
        IEnumerable<DbModel.SignalAPI> signalAPI = await _tradingLogicRepository.GetSignalAPI();
        IEnumerable<DbModel.StockTransactionDetails> stockTransactionDetails = await _tradingLogicRepository.GetStockTransactionDetails();

        StockSignalResponse stockSignalResponse = new();

        foreach(DbModel.Stock stock in stockList)
        {
            StockSignal stockSignal = new();
            stockSignal.Stock.Exchange=stock.Description??"NSE";
            stockSignal.Stock.Symbol=stock.Symbol;

            foreach(DbModel.SignalAPI signal in signalAPI)
            {
                stockSignal.SignalAPI.Add(new SignalAPI { Name = signal.Name, URL = signal.URL});
            }

            int holdingBuy = stockTransactionDetails.Where(t=>t.StockId==stock.Id && t.Type=="BUY").Sum(t=>t.Quantity);
            int holdingSell = stockTransactionDetails.Where(t=>t.StockId==stock.Id && t.Type=="SELL").Sum(t=>t.Quantity);
            int holding = holdingBuy - holdingSell;
            stockSignal.Holding=holding;

            stockSignalResponse.StockSignalList.Add(stockSignal);
        }

        return stockSignalResponse;
    }

    private async Task<StockSignalList> GetSignalAndSignalList()
    {
        IEnumerable<DbModel.Stock> stockList = await _tradingLogicRepository.GetStockList();
        IEnumerable<DbModel.SignalAPI> signalAPI = await _tradingLogicRepository.GetSignalAPI();

        StockSignalList stockSignalList = new ();
        stockSignalList.StockList = stockList.Select (t=> new Stock { Symbol = t.Symbol, Exchange = t.Description??"NSE" }).ToList();
        stockSignalList.SignalList = signalAPI.Select (t=> new Signal { Code = t.URL, Name = t.Name }).ToList();

        return stockSignalList;
    }

    public async Task<List<IndicatorSignalCustomView>> GetIndicatorSignalCustomView()
    {
        List<IndicatorSignalCustomView> indicatorSignalCustomList = new List<IndicatorSignalCustomView>();

        StockSignalList stockSignalList = await GetSignalAndSignalList();
        StockLiveDetailsResponse stockLiveDetailsResponse = await _tradingLogicClientService.GetStockLiveDetails(stockSignalList);
        var response = await _tradingLogicClientService.GetSignalBasedOnIndicator(stockSignalList);

        if (response.StockSignalList != null)
        {
            string stockName = string.Empty;
            IEnumerable<SignalDetail> signalDetailList;
            foreach (var stockSignal in response.StockSignalList)
            {
                foreach (var stock in stockSignal)
                {
                    stockName = stock.Key;
                    IndicatorSignalCustomView indicatorSignalCustomView1 = new()
                    {
                        Stock = stockName
                    };
                    indicatorSignalCustomList.Add(indicatorSignalCustomView1);

                    signalDetailList = stock.Value;
                    double? currentPrice = stockLiveDetailsResponse.StockLiveDetails.ContainsKey(stockName)?stockLiveDetailsResponse.StockLiveDetails[stockName].Price: null;
                    foreach (var signalDetail in signalDetailList)
                    {
                        IndicatorSignalCustomView indicatorSignalCustomView2 = new()
                        {
                            Stock = stockName,
                            Tool = signalDetail.Tool,
                            Signal = signalDetail.Signal,
                            Price = signalDetail.Price,
                            Date = signalDetail.Date,
                            CurrentPrice = currentPrice 
                        };
                        indicatorSignalCustomList.Add(indicatorSignalCustomView2);
                    }
                }
            }
        }

        return indicatorSignalCustomList;
    }

    public async Task<List<StockTransactionReportView>> GetStockTransactionDetails()
    {
        StockSignalList stockSignalList = await GetSignalAndSignalList();
        StockLiveDetailsResponse stockLiveDetailsResponse = await _tradingLogicClientService.GetStockLiveDetails(stockSignalList);
        IEnumerable<DbModel.StockTransactionDetails> transactionListDB = await _tradingLogicRepository.GetStockTransactionDetails();

        var stockTransactionReports = transactionListDB.GroupBy(a=> new { a.StockName }).Select(s => new StockTransactionReportView()
        {
            Stock = s.Key.StockName,
            Holding = s.Where(x=> x.Type == "Buy").Sum(x=> x.Quantity) - s.Where(x=> x.Type == "Sell").Sum(x=> x.Quantity),
            AvgPrice = (s.Where(x=> x.Type == "Buy").Sum(x=> x.Price) - s.Where(x=> x.Type == "Sell").Sum(x=> x.Price))/(s.Where(x=> x.Type == "Buy").Sum(x=> x.Quantity) - s.Where(x=> x.Type == "Sell").Sum(x=> x.Quantity)),
            CurrentPrice = stockLiveDetailsResponse.StockLiveDetails.ContainsKey(s.Key.StockName)?stockLiveDetailsResponse.StockLiveDetails[s.Key.StockName].Price:null,
            Profit = s.Where(x=> x.Type == "Sell").Sum(x=> x.Profit??0)
        }).ToList();

        return stockTransactionReports;
    }

    public async Task PopulateStockTransactionDetails(string stockName, string transType, int quantity, double price = 0, double? avgPrice = null)
    {

        await _tradingLogicRepository.PopulateStockTransactionDetails(stockName, transType, quantity, price, avgPrice);
    }
}