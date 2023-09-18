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

        // var test = _tradingLogicRepository.GetStockList().Result;

        // HttpClient client = new HttpClient();
        // var result = client.GetAsync("http://127.0.0.1:5000/get-user/4768");


        // var res = result.Result.Content.ReadAsStringAsync().Result;


        // List<SignalAPI> signalAPIList = new();
        // var signalAPIListFromDB = _tradingLogicRepository.GetSignalAPI().Result;
        // foreach(var signalAPI in signalAPIListFromDB)
        // {
        //     signalAPIList.Add(new SignalAPI(){ Name = signalAPI.Name, URL = signalAPI.URL });
        // }
        // ViewData["SignalAPIList"] = signalAPIList;

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
            // var response = await _tradingLogicClientService.GetSignalBasedOnIndicator();
            // if (response.StockSignalList != null)
            // {
            //     string stockName = string.Empty;
            //     IEnumerable<SignalDetail> signalDetailList;
            //     foreach (var stockSignal in response.StockSignalList)
            //     {
            //         foreach (var stock in stockSignal)
            //         {
            //             stockName = stock.Key;
            //             IndicatorSignalCustomView indicatorSignalCustomView1 = new();
            //             indicatorSignalCustomView1.Stock = stockName;
            //             indicatorSignalCustomList.Add(indicatorSignalCustomView1);

            //             signalDetailList = stock.Value;
            //             foreach (var signalDetail in signalDetailList)
            //             {
            //                 IndicatorSignalCustomView indicatorSignalCustomView2 = new();
            //                 indicatorSignalCustomView2.Stock = stockName;
            //                 indicatorSignalCustomView2.Tool = signalDetail.Tool;
            //                 indicatorSignalCustomView2.Signal = signalDetail.Signal;
            //                 indicatorSignalCustomView2.Price = signalDetail.Price;
            //                 //indicatorSignalCustomView2.OnDate = signalDetail.OnDate;
            //                 indicatorSignalCustomView2.Date = signalDetail.Date;
            //                 indicatorSignalCustomList.Add(indicatorSignalCustomView2);
            //             }
            //         }
            //     }
            
            //     _cacheService.SetTradingList<List<IndicatorSignalCustomView>>("Key", indicatorSignalCustomList);
            // }
        }

        if (!String.IsNullOrEmpty(searchString))
        {
            indicatorSignalCustomList = indicatorSignalCustomList.Where(s => s.Stock.Contains(searchString)).ToList();
        }

        //var signalStock = _tradingLogicService.StockSignalList().Result;
        //return View(indicatorSignalCustomList);

        int pageSize = 10;
        //var indicatorSignalCustomQuery = indicatorSignalCustomList.AsQueryable().Where(t=> 1==1);
        return View(PaginatedList<IndicatorSignalCustomView>.CreateAsync(indicatorSignalCustomList, pageNumber ?? 1, pageSize));
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
