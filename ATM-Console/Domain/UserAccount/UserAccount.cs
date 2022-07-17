namespace AtmConsole.Domain.UserAccounts
{
    public class UserAccount
    {
        public decimal AccountBalance { get; set; }
        public long AccountNumber { get; set; }
        public long CardNumber { get; set; }
        public int CardPin { get; set; }
        public string? FullName { get; set; }
        public bool IsLocked { get; set; }
        public int TotalLogin { get; set; }
        public int UserAccountId { get; set; }

        public UserAccount(int id, decimal accountBalance, long accountNumber, long cardNumber, int cardPin, string? fullName, bool isLocked, int totalLogin)
        {
            AccountBalance = accountBalance;
            AccountNumber = accountNumber;
            CardNumber = cardNumber;
            CardPin = cardPin;
            FullName = fullName;
            UserAccountId = id;
            IsLocked = isLocked;
            TotalLogin = totalLogin;
        }
    }
}
