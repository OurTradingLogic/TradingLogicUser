using Repository;
using Database;
using Microsoft.EntityFrameworkCore;
using Tests.Data;

namespace Tests;

public class TradingLogicRepositoryTest
{
    private readonly TradingLogicRepository _tradingLogicRepository;

    public TradingLogicRepositoryTest()
    {
        _tradingLogicRepository = CreateTradingLogicRepository();
    }

    private TradingLogicRepository CreateTradingLogicRepository()
    {
        return new TradingLogicRepository(CreateTradingLogicDbContext());
    }

    private TradingLogicDbContext CreateTradingLogicDbContext()
    {
        var dbOptions = new DbContextOptionsBuilder<TradingLogicDbContext>()
            .UseInMemoryDatabase("TradingLogic")
            .Options;

        var tradingLogicDbContext = new TradingLogicDbContext(dbOptions);

        TradingLogicRepositoryTestData.SeedDatabase(tradingLogicDbContext);

        return tradingLogicDbContext;
    }

    [Fact]
    public async void TestPopulateStockTransactionDetails()
    {
        string stockName = "Stock1"; 
        string transType = "Buy";
        int quantity = 1; 
        int price = 25;

        await _tradingLogicRepository.PopulateStockTransactionDetails(stockName, transType, quantity, price);

        var stockTransactionDetails = await _tradingLogicRepository.GetStockTransactionDetails();

        Assert.NotNull(stockTransactionDetails);
        var result = stockTransactionDetails.FirstOrDefault();
        Assert.True(result.Type == "Buy");
        Assert.True(result.Quantity == 1);
        Assert.True(result.Price == 25);
    }
}