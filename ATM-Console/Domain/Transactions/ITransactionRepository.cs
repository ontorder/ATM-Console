namespace AtmConsole.Domain.Transactions
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        IEnumerable<Transaction> Get();
    }
}
