using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database;
public class TradingLogicDbContext: DbContext
{
    public TradingLogicDbContext(DbContextOptions<TradingLogicDbContext> options): base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Users> Users {get; set;}

    public virtual DbSet<Stock> Stock {get; set;}

    public virtual DbSet<SignalAPI> SignalAPI {get; set;}

    public virtual DbSet<TransactionHistory> TransactionHistory {get; set;}
}
