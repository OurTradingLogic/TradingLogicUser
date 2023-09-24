using app.Services.Models;

namespace app.Services;
public interface ITradingLogicService
{
    Task<StockSignalResponse> StockSignalList();
    Task<List<IndicatorSignalCustomView>> GetIndicatorSignalCustomView();
    Task<List<StockTransactionReportView>> GetStockTransactionDetails();
    Task PopulateStockTransactionDetails(string stockName, string transType, int quantity, double price = 0, double? avgPrice = null);
}