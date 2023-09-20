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
        var result = await _tradingLogicDbContext.StockTransactionDetails.AsNoTracking().Where(x => x.UserId==userId).ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Users>> GetUsers()
    {
        var result = await _tradingLogicDbContext.Users.AsNoTracking().ToListAsync();

        return result;
    }
}
