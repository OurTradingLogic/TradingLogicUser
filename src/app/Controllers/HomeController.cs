﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Repository;
using app.Services;
using app.Services.Models;
using app.Services.Helpers;

namespace app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITradingLogicService _tradingLogicService;
    private readonly ICacheService _cacheService;

    public HomeController(ILogger<HomeController> logger, ITradingLogicService tradingLogicService, ICacheService cacheService)
    {
        _logger = logger;
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
            indicatorSignalCustomList = indicatorSignalCustomList.Where(s => s.Stock.ToLower().Contains(searchString.ToLower())).ToList();
        }

        int pageSize = 15;
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

    public async Task<IActionResult> BuyExecute(string stock, double price, int quantity)
    {
        await _tradingLogicService.PopulateStockTransactionDetails(stock, "Buy", quantity, price);

        return Json("Buy Succeed");
    } 

    public async Task<IActionResult> SellExecute(string stock, double price, double avgPrice, int quantity)
    {
        await _tradingLogicService.PopulateStockTransactionDetails(stock, "Sell", quantity, price, avgPrice);

        return Json("Sell Succeed");
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
