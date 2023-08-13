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

    public TradingLogicService(ILogger<TradingLogicService> logger, ITradingLogicRepository tradingLogicRepository)
    {
        _logger = logger;
        _tradingLogicRepository = tradingLogicRepository;
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

    public async Task<StockSignalList> GetSignalAndSignalList()
    {
        IEnumerable<DbModel.Stock> stockList = await _tradingLogicRepository.GetStockList();
        IEnumerable<DbModel.SignalAPI> signalAPI = await _tradingLogicRepository.GetSignalAPI();

        StockSignalList stockSignalList = new ();
        stockSignalList.StockList = stockList.Select (t=> new Stock { Symbol = t.Symbol, Exchange = t.Description??"NSE" }).ToList();
        stockSignalList.SignalList = signalAPI.Select (t=> new Signal { Code = t.URL, Name = t.Name }).ToList();

        return stockSignalList;
    }
}