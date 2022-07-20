using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.App
{
    public interface IAtmAppService
    {
        BankAccount? GetUserAccountByCardNumber(long cardNumberId);
        BankAccount? GetUserAccount(long accountId);
        void PlaceDeposit(long accountId, decimal amount, string? description);
        void Withdraw(long accountId, decimal amount, string? description);
        IEnumerable<Transaction> GetTransactions(long accountId);
        void Transfer(long fromAccountId, long toAccountId, decimal amount);
    }
}
