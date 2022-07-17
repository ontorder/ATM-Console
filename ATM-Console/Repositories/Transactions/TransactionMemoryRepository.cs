using AtmConsole.Domain.Transactions;

namespace AtmConsole.Repositories.Transactions
{
    public class TransactionMemoryRepository : ITransactionRepository
    {
        private long _autoIncrementId = 0;
        private readonly List<Transaction> _transactions = new();

        public IEnumerable<Transaction> Get()
            => _transactions;

        public IEnumerable<Transaction> Get(long accountId)
            => _transactions.Where(t => t.UserAccountId == accountId);

        public void Add(Transaction transaction)
        {
            if (transaction.TransactionId == default)
                transaction.TransactionId = ++_autoIncrementId;

            if (_transactions.Any(t => t.TransactionId == transaction.TransactionId))
                throw new InvalidOperationException($"TransactionId already exists [TransactionId: {transaction.TransactionId}]");

            _transactions.Add(transaction);
        }
    }
}
