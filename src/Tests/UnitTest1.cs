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

    
}