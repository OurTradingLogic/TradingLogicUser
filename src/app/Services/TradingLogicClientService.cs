using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using Repository;
using app.Services;
using System.Text.Json;
//using DbModel = Database.Models;
using app.Services.Models;
using DbModel = Database.Models;

namespace app.Services
{
    public interface ITradingLogicClientService
    {
        Task<IndicatorSignalResponse> GetSignalBasedOnIndicator();
    }

    public class TradingLogicClientService: ITradingLogicClientService
    {
        private readonly ILogger<TradingLogicClientService> _logger;
        private readonly ITradingLogicRepository _tradingLogicRepository;
        private readonly ITradingLogicService _tradingLogicService;
        private readonly IConfiguration _configuration;

        public TradingLogicClientService(ILogger<TradingLogicClientService> logger, ITradingLogicRepository tradingLogicRepository, ITradingLogicService tradingLogicService, IConfiguration configuration)
        {
            _logger = logger;
            _tradingLogicRepository = tradingLogicRepository;
            _tradingLogicService = tradingLogicService;
            _configuration = configuration;
        }

        public async Task<IndicatorSignalResponse> GetSignalBasedOnIndicator()
        {
            StockSignalList stockSignalList = await _tradingLogicService.GetSignalAndSignalList();

            string? apiServer = "http://127.0.0.1:5000";
            //azure call
            apiServer = _configuration.GetValue<string>("TradingLogicAPI");

            if (string.IsNullOrEmpty(apiServer))
                apiServer = "http://127.0.0.1:5000";

            string strJson = JsonSerializer.Serialize<StockSignalList>(stockSignalList);
            string apiURL = apiServer + "/getsignal-basedonindicator";

            HttpClient client = new HttpClient();
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
    }
}