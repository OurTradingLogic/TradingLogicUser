using Database;
using Database.Models;

namespace Repository;
public interface ITradingLogicRepository
{
    Task<IEnumerable<Stock>> GetStockList();
    Task<IEnumerable<SignalAPI>> GetSignalAPI();
    Task<IEnumerable<TransactionHistory>> GetTransactionHistory(int userId = 1);
    Task<IEnumerable<Users>> GetUsers();
}