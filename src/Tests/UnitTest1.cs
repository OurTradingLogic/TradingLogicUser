using Tests.Handlers;
using app.Services;
using app.Services.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DbModel = Database.Models;
using Repository;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public async void TestGetSignalBasedOnIndicator()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        var mockMessageHandler = fixture.Freeze<Mock<IMockHttpMessageHandler>>();
        var httpService = new HttpService(new MockHttpMessageHandler(mockMessageHandler.Object));
        var mockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        var mockLogger = fixture.Freeze<Mock<ILogger<TradingLogicClientService>>>();

        var clientResponse = "{\n  \"AEGISCHEM.NS\": [\n {\n      \"Date\": \"2023-06-23\",\n      \"OnDate\": \"2023-07-21\",\n      \"Price\": 317.45,\n      \"Signal\": \"BUY\",\n      \"Tool\": \"BBANDDAILY\"\n    },\n    {\n      \"Date\": \"2023-07-21\",\n      \"OnDate\": \"2023-07-21\",\n      \"Price\": 355.65,\n      \"Signal\": \"BUY\",\n      \"Tool\": \"TRENDDAILY\"\n    }\n  ]\n}\n";

        mockMessageHandler.Setup(a=>a.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
        .Returns(Task.Run(() => new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(clientResponse, Encoding.UTF8, "application/json")
        }));

        var tradingLogicClientService = new TradingLogicClientService(mockLogger.Object, mockConfiguration.Object, httpService);

        StockSignalList stockSignalList = new StockSignalList();

        var signalBasedOnIndicator = await tradingLogicClientService.GetSignalBasedOnIndicator(stockSignalList);

        Assert.NotNull(signalBasedOnIndicator);
        Assert.NotNull(signalBasedOnIndicator.StockSignalList);
        Assert.True(signalBasedOnIndicator.StockSignalList.Any());      
    }

    [Fact]
    public async void TestGetStockLiveDetails()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        var mockMessageHandler = fixture.Freeze<Mock<IMockHttpMessageHandler>>();
        var httpService = new HttpService(new MockHttpMessageHandler(mockMessageHandler.Object));
        var mockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        var mockLogger = fixture.Freeze<Mock<ILogger<TradingLogicClientService>>>();

        //var clientResponse = "{\n  \"AEGISCHEM.NS\": {\n      \"Price\": 317.45    }\n }\n";

        var clientResponse = "{\n \"AEGISCHEM\": {\n\"Price\": 332.20001220703125}\n,\"AMARAJABAT\": {\n "+
        "\"Price\": 646.25}\n,\"APOLLOTYRE\": {\n\"Price\": 374.04998779296875}\n,\"ASIANPAINT\": {\n"+
        "\"Price\": 3200.5}\n,\"AUROPHARMA\": {\n\"Price\": 896.4000244140625}\n,\"AXISBANK\": {\n"+
        "\"Price\": 1039.5999755859375}\n,\"BATAINDIA\": {\n}\n,\"BERGEPAINT\": {\n"+
        "\"Price\": 739.3499755859375}\n,\"BHARATFORG\": {\n\"Price\": 1126.5999755859375}\n,\"BLUEDART\": {\n"+
        "\"Price\": 6857.7998046875}\n,\"BPCL\": {\n\"Price\": 354.5}\n,\"CASTROLIND\": {\n"+
        "\"Price\": 145.39999389648438}\n,\"CHENNPETRO\": {\n\"Price\": 541.9500122070312}\n,\"CPSEETF\": {\n"+
        "\"Price\": 52.95000076293945}\n,\"CUMMINSIND\": {\n\"Price\": 1749.199951171875}\n,\"Cipla\": {\n"+
        "\"Price\": 1243.949951171875}\n,\"DCBBANK\": {\n\"Price\": 123.0}\n,\"DIVISLAB\": {\n"+
        "\"Price\": 3787.25}\n,\"DLF\": {\n\"Price\": 527.4000244140625}\n,\"EVEREADY\": {\n\"Price\": 401.75}\n}\n";

        mockMessageHandler.Setup(a=>a.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
        .Returns(Task.Run(() => new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(clientResponse, Encoding.UTF8, "application/json")
        }));

        var tradingLogicClientService = new TradingLogicClientService(mockLogger.Object, mockConfiguration.Object, httpService);

        StockSignalList stockSignalList = new StockSignalList();

        var stockLiveDetails = await tradingLogicClientService.GetStockLiveDetails(stockSignalList);

        Assert.NotNull(stockLiveDetails);
        Assert.NotNull(stockLiveDetails.StockLiveDetails);
        Assert.True(stockLiveDetails.StockLiveDetails.Any());      
    }

    [Fact]
    public async void TestGetStockTransactionDetails()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        var mockRepository = fixture.Freeze<Mock<ITradingLogicRepository>>();
        var mockTradingLogicClientService = fixture.Freeze<Mock<ITradingLogicClientService>>();
        var mockLogger = fixture.Freeze<Mock<ILogger<TradingLogicService>>>();

        //var clientResponse = "{\n  \"AEGISCHEM.NS\": {\n      \"Price\": 317.45    }\n }\n";
        //string response = "[" +clientResponse+ "]";
        var output = JsonSerializer.Deserialize<Dictionary<string, StockLiveDetail>>(TestDataConstants.StockLiveDetailsResponse);
        StockLiveDetailsResponse stockLiveResponse = new StockLiveDetailsResponse();
        stockLiveResponse.StockLiveDetails = output;
        mockTradingLogicClientService.Setup(a=>a.GetStockLiveDetails(It.IsAny<StockSignalList>())).ReturnsAsync(stockLiveResponse);

        List<DbModel.StockTransactionDetails> stockTransactionDetailsDB = new List<DbModel.StockTransactionDetails>() {
            new DbModel.StockTransactionDetails() { Id = 1, UserId = 1, StockId = 1, StockName = "AEGISCHEM", Type = "Buy", Quantity = 1, DateAndTime = DateTime.Now.AddDays(-1), Price = 10 },
            new DbModel.StockTransactionDetails() { Id = 2, UserId = 1, StockId = 1, StockName = "AEGISCHEM", Type = "Sell", Quantity = 1, DateAndTime = DateTime.Now.AddDays(-11), Price = 15, Profit = 5  },
            new DbModel.StockTransactionDetails() { Id = 3, UserId = 1, StockId = 1, StockName = "AEGISCHEM", Type = "Buy", Quantity = 3, DateAndTime = DateTime.Now, Price = 24 },
            new DbModel.StockTransactionDetails() { Id = 4, UserId = 1, StockId = 2, StockName = "APOLLOTYRE", Type = "Buy", Quantity = 10, DateAndTime = DateTime.Now.AddDays(-5), Price = 150 },
            new DbModel.StockTransactionDetails() { Id = 5, UserId = 1, StockId = 2, StockName = "APOLLOTYRE", Type = "Sell", Quantity = 5, DateAndTime = DateTime.Now.AddDays(-2), Price = 50, Profit = 5  },
        };
        mockRepository.Setup(a=>a.GetStockTransactionDetails(It.IsAny<int>())).ReturnsAsync(stockTransactionDetailsDB);

        var tradingLogicService = new TradingLogicService(mockLogger.Object, mockRepository.Object, mockTradingLogicClientService.Object);

        var stockTransactionDetails = await tradingLogicService.GetStockTransactionDetails();

        Assert.NotNull(stockTransactionDetails); 
        Assert.True(stockTransactionDetails.Any()); 
        var stock1 = stockTransactionDetails.Where(w=>w.Stock == "AEGISCHEM").FirstOrDefault();
        Assert.NotNull(stock1); 
        Assert.True(stock1.Holding == 3); 
        Assert.True(stock1.Profit == 5); 
        Assert.True((int)stock1.AvgPrice == 6); 
        var stock2 = stockTransactionDetails.Where(w=>w.Stock == "APOLLOTYRE").FirstOrDefault();
        Assert.NotNull(stock2); 
        Assert.True(stock2.Holding == 5); 
        Assert.True(stock2.Profit == 5); 
        Assert.True((int)stock2.AvgPrice == 20); 
    }
}