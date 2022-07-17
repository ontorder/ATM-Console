using AtmConsole.Domain.Transactions;

namespace AtmConsole.Domain.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(long userBankAccountId, TransactionType transactionType, decimal transactionAmount, string description);
        void ViewTransaction();
    }
}
