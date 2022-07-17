using AtmConsole.Domain.Enums;

namespace AtmConsole.Domain.Interfaces
{
    internal interface ITransaction
    {
        void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);
        void ViewTransaction();
    }
}
