using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Repositories
{
    public class AtmConsoleContext
    {
        public readonly ITransactionRepository Transactions;
        public readonly IUserAccountRepository UserAccounts;

        public AtmConsoleContext(ITransactionRepository transactions, IUserAccountRepository userAccounts)
        {
            Transactions = transactions;
            UserAccounts = userAccounts;
        }
    }
}
