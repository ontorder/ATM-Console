namespace AtmConsole.Domain.Entities
{
    public class InternalTransfer
    {
        public string RecipientBankAccountName { get; set; }
        public long RecipientBankAccountNumber { get; set; }
        public decimal TransferAmount { get; set; }
    }
}
