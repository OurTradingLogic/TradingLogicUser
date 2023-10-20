using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class TradingLogicRepository: ITradingLogicRepository
{
    private readonly TradingLogicDbContext _tradingLogicDbContext;

    public TradingLogicRepository(TradingLogicDbContext tradingLogicDbContext)
    {
        _tradingLogicDbContext = tradingLogicDbContext;
    }

    public async Task<IEnumerable<Stock>> GetStockList()
    {
        //var result = await _tradingLogicDbContext.Stock.AsNoTracking().ToListAsync();

        List<Stock> stocks = new List<Stock> {
            new Stock {
                Symbol = "AEGISCHEM",
                Name = "Aegis Logistics",
                Description = "NSE"
            },
            new Stock {
                Symbol = "AMARAJABAT",
                Name = "Amara Raja Battery",
                Description = "NSE"
            },
            new Stock {
                Symbol = "APOLLOTYRE",
                Name = "APOLLOTYRE",
                Description = "NSE"
            },
            new Stock {
                Symbol = "ASIANPAINT",
                Name = "ASIANPAINT",
                Description = "NSE"
            },
            new Stock {
                Symbol = "AUROPHARMA",
                Name = "AUROPHARMA",
                Description = "NSE"
            },
            new Stock {
                Symbol = "AXISBANK",
                Name = "AXISBANK",
                Description = "NSE"
            },
            new Stock {
                Symbol = "BATAINDIA",
                Name = "BATAINDIA",
                Description = "NSE"
            },
            new Stock {
                Symbol = "BERGEPAINT",
                Name = "BERGEPAINT",
                Description = "NSE"
            },
            new Stock {
                Symbol = "BHARATFORG",
                Name = "BHARATFORG",
                Description = "NSE"
            },
            new Stock {
                Symbol = "BPCL",
                Name = "BPCL",
                Description = "NSE"
            },
            new Stock {
                Symbol = "BLUEDART",
                Name = "BLUEDART",
                Description = "NSE"
            },
            new Stock {
                Symbol = "CASTROLIND",
                Name = "CASTROLIND",
                Description = "NSE"
            },
            new Stock {
                Symbol = "CHENNPETRO",
                Name = "CHENNPETRO",
                Description = "NSE"
            },
            new Stock {
                Symbol = "Cipla",
                Name = "Cipla",
                Description = "NSE"
            },
            new Stock {
                Symbol = "CPSEETF",
                Name = "CPSEETF",
                Description = "NSE"
            },
            new Stock {
                Symbol = "CUMMINSIND",
                Name = "CUMMINSIND",
                Description = "NSE"
            },
            new Stock {
                Symbol = "DCBBANK",
                Name = "DCBBANK",
                Description = "NSE"
            },
            new Stock {
                Symbol = "DIVISLAB",
                Name = "DIVISLAB",
                Description = "NSE"
            },
            new Stock {
                Symbol = "DLF",
                Name = "DLF",
                Description = "NSE"
            },
            new Stock {
                Symbol = "EVEREADY",
                Name = "EVEREADY",
                Description = "NSE"
            },
            new Stock {
                Symbol = "EXIDEIND",
                Name = "Exide Battery",
                Description = "NSE"
            },
            new Stock {
                Symbol = "FINCABLES",
                Name = "Finolex Cables",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GAIL",
                Name = "GAIL",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GHCL",
                Name = "GHCL",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GNFC",
                Name = "GNFC",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GODREJCP",
                Name = "Godrej Consumer",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GODREJIND",
                Name = "Godrej Industries",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GODREJPROP",
                Name = "Godrej Properties Limited",
                Description = "NSE"
            },
            new Stock {
                Symbol = "GODREJAGRO",
                Name = "GODREJAGRO",
                Description = "NSE"
            },
            new Stock {
                Symbol = "Grasim",
                Name = "Grasim",
                Description = "NSE"
            },
            new Stock {
                Symbol = "HDFCBANK",
                Name = "HDFCBANK",
                Description = "NSE"
            },
            new Stock {
                Symbol = "HDFCLIFE",
                Name = "HDFCLIFE",
                Description = "NSE"
            },
            new Stock {
                Symbol = "HNDFDS",
                Name = "HNDFDS",
                Description = "NSE"
            },
            new Stock {
                Symbol = "HINDPETRO",
                Name = "HINDPETRO",
                Description = "NSE"
            },
            new Stock {
                Symbol = "IDFCFIRSTB",
                Name = "IDFCFIRSTB",
                Description = "NSE"
            },
            new Stock {
                Symbol = "IOC",
                Name = "Indian Oil Corporation",
                Description = "NSE"
            },
            new Stock {
                Symbol = "INDUSINDBK",
                Name = "INDUSINDBK",
                Description = "NSE"
            },
            new Stock {
                Symbol = "INFY",
                Name = "INFY",
                Description = "NSE"
            },
            new Stock {
                Symbol = "IRCTC",
                Name = "IRCTC",
                Description = "NSE"
            },
            new Stock {
                Symbol = "ITC",
                Name = "ITC",
                Description = "NSE"
            },
            new Stock {
                Symbol = "KOTAKBANK",
                Name = "KOTAKBANK",
                Description = "NSE"
            },
            new Stock {
                Symbol = "LT",
                Name = "LT",
                Description = "NSE"
            },
            new Stock {
                Symbol = "M&M",
                Name = "M&M",
                Description = "NSE"
            },
            new Stock {
                Symbol = "MARUTI",
                Name = "MARUTI",
                Description = "NSE"
            },
            new Stock {
                Symbol = "MSUMI",
                Name = "MSUMI",
                Description = "NSE"
            },
            new Stock {
                Symbol = "NHPC",
                Name = "NHPC",
                Description = "NSE"
            },
            new Stock {
                Symbol = "OIL",
                Name = "Oil India Limited",
                Description = "NSE"
            },
            new Stock {
                Symbol = "ONGC",
                Name = "ONGC",
                Description = "NSE"
            },
            new Stock {
                Symbol = "PAYTM",
                Name = "PAYTM",
                Description = "NSE"
            },
            new Stock {
                Symbol = "PETRONET",
                Name = "PETRONET",
                Description = "NSE"
            },
            new Stock {
                Symbol = "POWERGRID",
                Name = "POWERGRID",
                Description = "NSE"
            },
            new Stock {
                Symbol = "RAMCOCEM",
                Name = "RAMCOCEM",
                Description = "NSE"
            },
            new Stock {
                Symbol = "SAIL",
                Name = "SAIL",
                Description = "NSE"
            },
            new Stock {
                Symbol = "SBILIFE",
                Name = "SBILIFE",
                Description = "NSE"
            },
            new Stock {
                Symbol = "SJVN",
                Name = "SJVN",
                Description = "NSE"
            },
            new Stock {
                Symbol = "SONATSOFTW",
                Name = "SONATSOFTW",
                Description = "NSE"
            },
            new Stock {
                Symbol = "SUNTV",
                Name = "SUNTV",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TATACOFFEE",
                Name = "TATACOFFEE",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TATACOMM",
                Name = "TATACOMM",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TCS",
                Name = "TCS",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TATACONSUM",
                Name = "TATACONSUM",
                Description = "NSE"
            },
            new Stock {
                Symbol = "Tatamotors",
                Name = "Tatamotors",
                Description = "NSE"
            },
            new Stock {
                Symbol = "Tatasteel",
                Name = "Tatasteel",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TATAPOWER",
                Name = "TATAPOWER",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TORNTPHARM",
                Name = "TORNTPHARM",
                Description = "NSE"
            },
            new Stock {
                Symbol = "TVSMOTOR",
                Name = "TVSMOTOR",
                Description = "NSE"
            },
            new Stock {
                Symbol = "ULTRACEMCO",
                Name = "ULTRACEMCO",
                Description = "NSE"
            },
            new Stock {
                Symbol = "HINDUNILVR",
                Name = "HINDUNILVR",
                Description = "NSE"
            },
            new Stock {
                Symbol = "VOLTAS",
                Name = "VOLTAS",
                Description = "NSE"
            },
            new Stock {
                Symbol = "ZEEL",
                Name = "Zee Entertainment",
                Description = "NSE"
            }
            
        };
        var result =stocks.ToList(); 
        return result;
    }

    public async Task<IEnumerable<SignalAPI>> GetSignalAPI()
    {
        //var result = await _tradingLogicDbContext.SignalAPI.AsNoTracking().ToListAsync();

        List<SignalAPI> signals = new List<SignalAPI> {
            new SignalAPI { 
                Name = "Bollinger Band",
                URL = "BBAND"
            },
            new SignalAPI {
                Name = "Trend",
                URL = "TREND"
            },
            new SignalAPI {
                Name = "Support And Resistence",
                URL = "SUPPORTRESISTENCE"
            }
        };

        var result = signals.ToList();

        return result;
    }

    public async Task<IEnumerable<StockTransactionDetails>> GetStockTransactionDetails(int userId = 1)
    {
        var result = await _tradingLogicDbContext.StockTransactionDetails.AsNoTracking().Where(x => x.UserId==userId)?.ToListAsync();

        return result;
    }

    public async Task PopulateStockTransactionDetails(string stockName, string transType, int quantity, double price = 0, double? avgPrice = null, int userId = 1)
    {
        double profit = 0;

        if (transType == "Sell")
        {
            double calAvgPrice = avgPrice??price;
            if (avgPrice == null)
            {
                var getStockTransactionDetails = await GetStockTransactionDetails();
                calAvgPrice = getStockTransactionDetails.Where(s=> s.StockName == stockName && s.Type == "Buy")
                                .GroupBy(g => g.StockName).Select(s=> s.Average(x=>x.Price)).FirstOrDefault();
            }

            profit = price - calAvgPrice;
        }

        StockTransactionDetails stockTransactionDetails = new StockTransactionDetails(){
            UserId = userId,
            StockId = 0,
            StockName = stockName,
            Type = transType,
            Quantity = quantity,
            DateAndTime = DateTime.Now,
            Price = price > 0 ? price * quantity : 0,
            Profit = profit
        };

        _tradingLogicDbContext.StockTransactionDetails.Add(stockTransactionDetails);

        await _tradingLogicDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Users>> GetUsers()
    {
        var result = await _tradingLogicDbContext.Users.AsNoTracking().ToListAsync();

        return result;
    }
}
