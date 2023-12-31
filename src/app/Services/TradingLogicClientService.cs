using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using Repository;
using app.Services;
using System.Text.Json;
using app.Services.Models;
using DbModel = Database.Models;

namespace app.Services
{
    public interface ITradingLogicClientService
    {
        Task<IndicatorSignalResponse> GetSignalBasedOnIndicator(StockSignalList stockSignalList);
        Task<StockLiveDetailsResponse> GetStockLiveDetails(StockSignalList stockSignalList);
    }

    public class TradingLogicClientService: ITradingLogicClientService
    {
        private readonly ILogger<TradingLogicClientService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpService _httpService;

        public TradingLogicClientService(ILogger<TradingLogicClientService> logger, IConfiguration configuration, IHttpService httpService)
        {
            _logger = logger;
            _configuration = configuration;
            _httpService = httpService;
        }

        public async Task<IndicatorSignalResponse> GetSignalBasedOnIndicator(StockSignalList stockSignalList)
        {
            string? apiServer = "http://127.0.0.1:5000";
            //azure call
            apiServer = _configuration.GetValue<string>("TradingLogicAPI");

            if (string.IsNullOrEmpty(apiServer))
                apiServer = "http://127.0.0.1:5000";

            string strJson = JsonSerializer.Serialize<StockSignalList>(stockSignalList);
            string apiURL = apiServer + "/getsignal-basedonindicator";

            //HttpClient client = new HttpClient();
            var client = _httpService;
            var result = await client.PostAsJsonAsync(apiURL, strJson);

            IndicatorSignalResponse stockSignalResponse = new();

            string jsonResponse = result.Content.ReadAsStringAsync().Result;

            //var response = "[{\n  \"AEGISCHEM.NS\": [\n {\n      \"Date\": \"2023-06-23\",\n      \"OnDate\": \"2023-07-21\",\n      \"Price\": 317.45,\n      \"Signal\": \"BUY\",\n      \"Tool\": \"BBANDDAILY\"\n    },\n    {\n      \"Date\": \"2023-07-21\",\n      \"OnDate\": \"2023-07-21\",\n      \"Price\": 355.65,\n      \"Signal\": \"BUY\",\n      \"Tool\": \"TRENDDAILY\"\n    }\n  ]\n}\n]";

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                string response = "[" +jsonResponse+ "]";
                var output = JsonSerializer.Deserialize<List<Dictionary<string, IEnumerable<SignalDetail>>>>(response);
                stockSignalResponse.StockSignalList = output;
            }

            return stockSignalResponse;
        }

        public async Task<StockLiveDetailsResponse> GetStockLiveDetails(StockSignalList stockSignalList)
        {
            string? apiServer = "http://127.0.0.1:5000";
            //azure call
            apiServer = _configuration.GetValue<string>("TradingLogicAPI");

            if (string.IsNullOrEmpty(apiServer))
                apiServer = "http://127.0.0.1:5000";

            string strJson = JsonSerializer.Serialize<StockSignalList>(stockSignalList);
            string apiURL = apiServer + "/getstocks-livedetails";

            //HttpClient client = new HttpClient();
            var client = _httpService;
            var result = await client.PostAsJsonAsync(apiURL, strJson);

            StockLiveDetailsResponse stockLiveResponse = new();

            string jsonResponse = result.Content.ReadAsStringAsync().Result;

            //var response = "[{\n  \"AEGISCHEM.NS\": [\n {\n      \"Price\": 317.45    }\n  ]\n}\n]";

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                //string response = "[" +jsonResponse+ "]";
                var output = JsonSerializer.Deserialize<Dictionary<string, StockLiveDetail>>(jsonResponse);
                stockLiveResponse.StockLiveDetails = output;
            }

            return stockLiveResponse;
        }
    }
}