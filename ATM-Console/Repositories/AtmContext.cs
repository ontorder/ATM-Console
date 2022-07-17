using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Repositories
{
    public class AtmContext
    {
        public readonly ITransactionRepository Transactions;
        public readonly IUserAccountRepository UserAccounts;

        public AtmContext(ITransactionRepository transactions, IUserAccountRepository userAccounts)
        {
            Transactions = transactions;
            UserAccounts = userAccounts;
        }
    }
}
