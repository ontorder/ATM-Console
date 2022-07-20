namespace AtmConsole.Domain.BankAccounts
{
    public class BankAccount
    {
        public decimal AccountBalance;
        public long AccountNumber;
        public int BankAccountId;
        public long UserId;

        // delete
        public int AuthFailsCount;
        public string FullName;
        public long CardNumber;
        public int CardPin;
        public bool IsLocked;

        public BankAccount(int id, decimal accountBalance, long accountNumber, long cardNumber, int cardPin, string fullName, bool isLocked, int authFailsCount)
        {
            AccountBalance = accountBalance;
            AccountNumber = accountNumber;
            CardNumber = cardNumber;
            CardPin = cardPin;
            FullName = fullName;
            BankAccountId = id;
            IsLocked = isLocked;
            AuthFailsCount = authFailsCount;
        }
    }
}
