namespace AtmConsole.Domain.UserAccounts
{
    public class UserAccount
    {
        public decimal AccountBalance;
        public long AccountNumber;
        public int AuthFailsCount;
        public long CardNumber;
        public int CardPin;
        public string FullName;
        public bool IsLocked;
        public int UserAccountId;

        public UserAccount(int id, decimal accountBalance, long accountNumber, long cardNumber, int cardPin, string fullName, bool isLocked, int authFailsCount)
        {
            AccountBalance = accountBalance;
            AccountNumber = accountNumber;
            CardNumber = cardNumber;
            CardPin = cardPin;
            FullName = fullName;
            UserAccountId = id;
            IsLocked = isLocked;
            AuthFailsCount = authFailsCount;
        }
    }
}
