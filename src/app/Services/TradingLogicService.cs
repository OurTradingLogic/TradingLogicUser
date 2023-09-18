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
        IEnumerable<DbModel.TransactionHistory> transactionHistory = await _tradingLogicRepository.GetTransactionHistory();

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

            int holdingBuy = transactionHistory.Where(t=>t.StockId==stock.Id && t.Type=="BUY").Sum(t=>t.Quantity);
            int holdingSell = transactionHistory.Where(t=>t.StockId==stock.Id && t.Type=="SELL").Sum(t=>t.Quantity);
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
                    IndicatorSignalCustomView indicatorSignalCustomView1 = new();
                    indicatorSignalCustomView1.Stock = stockName;
                    indicatorSignalCustomList.Add(indicatorSignalCustomView1);

                    signalDetailList = stock.Value;
                    foreach (var signalDetail in signalDetailList)
                    {
                        IndicatorSignalCustomView indicatorSignalCustomView2 = new();
                        indicatorSignalCustomView2.Stock = stockName;
                        indicatorSignalCustomView2.Tool = signalDetail.Tool;
                        indicatorSignalCustomView2.Signal = signalDetail.Signal;
                        indicatorSignalCustomView2.Price = signalDetail.Price;
                        //indicatorSignalCustomView2.OnDate = signalDetail.OnDate;
                        indicatorSignalCustomView2.Date = signalDetail.Date;
                        indicatorSignalCustomList.Add(indicatorSignalCustomView2);
                    }
                }
            }
        }

        return indicatorSignalCustomList;
    }
}