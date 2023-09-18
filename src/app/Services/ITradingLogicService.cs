using app.Services.Models;

namespace app.Services;
public interface ITradingLogicService
{
    Task<StockSignalResponse> StockSignalList();
    Task<List<IndicatorSignalCustomView>> GetIndicatorSignalCustomView();
}