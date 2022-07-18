using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.UserAccounts;
using AtmConsole.Repositories;

namespace AtmConsole.App
{
    public class AtmAppService : IAtmAppService
    {
        private readonly AtmContext _context;

        public AtmAppService(AtmContext context)
        {
            _context = context;
        }

        public UserAccount? GetUserAccountByCardNumber(long cardNumber)
            => _context.UserAccounts.Search(cardNumber);

        public void PlaceDeposit(long accountId, decimal amount, string? description)
        {
            _context.Transactions.Add(new Transaction(default, description, amount, DateTime.Now, TransactionType.Deposit, accountId));
            var account = _context.UserAccounts.Find(accountId);
            account.AccountBalance += amount;
        }

        public void Withdraw(long accountId, decimal amount, string? description)
        {
            _context.Transactions.Add(new Transaction(default, description, -amount, DateTime.Now, TransactionType.Withdrawal, accountId));
            var account = _context.UserAccounts.Find(accountId);

            // TODO could go <0 depending on user roles?
            if (amount > account.AccountBalance)
            {
                throw new Exception("Not enough money");
            }
            account.AccountBalance -= amount;
        }

        public IEnumerable<Transaction> GetTransactions(long accountId)
            => _context.Transactions.Get(accountId);

        public void Transfer(long fromAccountId, long toAccountId, decimal amount)
        {
            // TODO controls
            var fromAccount = _context.UserAccounts.Find(fromAccountId);
            var toAccount = _context.UserAccounts.Find(toAccountId);
            DateTime timestamp = DateTime.Now;
            _context.Transactions.Add(new Transaction(default,
                $"Transferred Amount to {toAccount.FullName} {toAccount.AccountNumber}",
                -amount, timestamp, TransactionType.Transfer, fromAccountId
            ));
            fromAccount.AccountBalance -= amount;
            _context.Transactions.Add(new Transaction(
                default,
                $"Received Amount from {fromAccount.FullName} {fromAccount.AccountNumber}",
                amount, timestamp, TransactionType.Transfer, toAccountId));
            toAccount.AccountBalance += amount;
        }

        public UserAccount? GetUserAccount(long accountId)
            => _context.UserAccounts.Find(accountId);
    }
}
