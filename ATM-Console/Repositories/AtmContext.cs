using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.Repositories
{
    public class AtmContext
    {
        public readonly ITransactionRepository Transactions;
        public readonly IBankAccountRepository UserAccounts;

        public AtmContext(ITransactionRepository transactions, IBankAccountRepository userAccounts)
        {
            Transactions = transactions;
            UserAccounts = userAccounts;
        }
    }
}
