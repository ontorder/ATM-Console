using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.App
{
    public interface IAtmAppService
    {
        UserAccount? GetUserAccountByCardNumber(long cardNumberId);
        UserAccount? GetUserAccount(long accountId);
        void PlaceDeposit(long accountId, decimal amount, string? description);
        void Withdraw(long accountId, decimal amount, string? description);
        IEnumerable<Transaction> GetTransactions(long accountId);
        void Transfer(long fromAccountId, long toAccountId, decimal amount);
    }
}
