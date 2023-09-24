using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Repository;
using app.Services;
using app.Services.Models;
using app.Services.Helpers;
using System.Web;

namespace app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITradingLogicRepository _tradingLogicRepository;
    private readonly ITradingLogicService _tradingLogicService;
    private readonly ICacheService _cacheService;

    public HomeController(ILogger<HomeController> logger, ITradingLogicRepository tradingLogicRepository, ITradingLogicService tradingLogicService, ICacheService cacheService)
    {
        _logger = logger;
        _tradingLogicRepository = tradingLogicRepository;
        _tradingLogicService = tradingLogicService;
        _cacheService = cacheService;
    }

    public async Task<IActionResult> Index(string searchString, int? pageNumber)
    {
        ViewData["CurrentFilter"] = searchString;
        if (searchString != null)
        {
            pageNumber = 1;
        }

        List<IndicatorSignalCustomView> indicatorSignalCustomList = new List<IndicatorSignalCustomView>();

        var indicatorSignalCustomListFromCache = _cacheService.GetTradingList<List<IndicatorSignalCustomView>>("Key");
        if (indicatorSignalCustomListFromCache != null && indicatorSignalCustomListFromCache.Count() > 0)
        {
            indicatorSignalCustomList = indicatorSignalCustomListFromCache;
        }

        if (indicatorSignalCustomList.Count == 0)
        {
            var indicatorSignalCustomView = await _tradingLogicService.GetIndicatorSignalCustomView();

            if (indicatorSignalCustomView.Any())
            {
                _cacheService.SetTradingList<List<IndicatorSignalCustomView>>("Key", indicatorSignalCustomView);
            }
        }

        if (!String.IsNullOrEmpty(searchString))
        {
            indicatorSignalCustomList = indicatorSignalCustomList.Where(s => s.Stock.Contains(searchString)).ToList();
        }

        int pageSize = 10;
        return View(PaginatedList<IndicatorSignalCustomView>.CreateAsync(indicatorSignalCustomList, pageNumber ?? 1, pageSize));
    }

    public async Task<IActionResult> StockTransactionReport(string searchString, int? pageNumber)
    {
        ViewData["CurrentFilter"] = searchString;
        if (searchString != null)
        {
            pageNumber = 1;
        }

        List<StockTransactionReportView> stockTransactionReportList = await _tradingLogicService.GetStockTransactionDetails();

        if (!String.IsNullOrEmpty(searchString))
        {
            stockTransactionReportList = stockTransactionReportList.Where(s => s.Stock.Contains(searchString)).ToList();
        }

        int pageSize = 10;
        return View(PaginatedList<StockTransactionReportView>.CreateAsync(stockTransactionReportList, pageNumber ?? 1, pageSize));
    }

    public async Task<IActionResult> Buy(string stock, double price)
    {
        await _tradingLogicService.PopulateStockTransactionDetails(stock, "Buy", 1, price);

        return View();
    } 

    public async Task<IActionResult> Sell(string stock, double price, double avgPrice)
    {
        await _tradingLogicService.PopulateStockTransactionDetails(stock, "Sell", 1, price, avgPrice);

        return View();
    } 

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
