namespace AtmConsole.Domain.InternalTransfers
{
    public class InternalTransfer
    {
        public string RecipientBankAccountName { get; set; }
        public long RecipientBankAccountNumber { get; set; }
        public decimal TransferAmount { get; set; }

        public InternalTransfer(string recipientBankAccountName, long recipientBankAccountNumber, decimal transferAmount)
        {
            RecipientBankAccountName = recipientBankAccountName;
            RecipientBankAccountNumber = recipientBankAccountNumber;
            TransferAmount = transferAmount;
        }
    }
}
