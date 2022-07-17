namespace AtmConsole.Domain.Transactions
{
    public class Transaction
    {
        public string? Description { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public long UserAccountId { get; set; }

        public Transaction(long transactionId, string? description, decimal transactionAmount, DateTime transactionDate, TransactionType transactionType, long userAccountId)
        {
            Description = description;
            TransactionAmount = transactionAmount;
            TransactionDate = transactionDate;
            TransactionId = transactionId;
            TransactionType = transactionType;
            UserAccountId = userAccountId;
        }
    }
}
