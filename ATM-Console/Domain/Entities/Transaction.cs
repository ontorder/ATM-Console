using AtmConsole.Domain.Enums;

namespace AtmConsole.Domain.Entities
{
    internal class Transaction
    {
        public string? Description { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public long UserBankAccountID { get; set; }
    }
}
