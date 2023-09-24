using Database;

namespace Tests.Data;

public static class TradingLogicRepositoryTestData
{
    private static readonly object _lock = new object();
    private static bool _initialized = false;

    public static void SeedDatabase(TradingLogicDbContext tradingLogicDbContext)
    {
        lock(_lock)
        {
            if (!_initialized)
            {

                _initialized = true;
            }
        }
    }
}