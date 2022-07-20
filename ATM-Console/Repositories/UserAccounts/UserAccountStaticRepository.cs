using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.Repositories.UserAccounts
{
    public class UserAccountStaticRepository : IBankAccountRepository
    {
        private List<BankAccount> _userAccounts;

        public UserAccountStaticRepository()
        {
            _userAccounts = new List<BankAccount>
            {
                new BankAccount(
                    id: 1,
                    accountBalance: 100000.00m,
                    accountNumber: 543210,
                    cardNumber: 123123,
                    cardPin: 321321,
                    fullName: "Uzumaki Naruto",
                    isLocked: false,
                    authFailsCount: 0
                ),

                new BankAccount(
                    id: 2,
                    accountBalance: 40000.00m,
                    accountNumber: 987654,
                    cardNumber: 987987,
                    cardPin: 789789,
                    fullName: "Hatake Kakashi",
                    isLocked: false,
                    authFailsCount: 0
                ),

                new BankAccount(
                    id: 3,
                    accountBalance: 150000.00m,
                    accountNumber: 975310,
                    cardNumber: 975975,
                    cardPin: 579579,
                    fullName: "Hyuga Hinata",
                    isLocked: true,
                    authFailsCount: 0
                )
            };
        }

        public IEnumerable<BankAccount> Get()
            => _userAccounts;

        public BankAccount? Search(long cardNumber)
        {
            var account = _userAccounts.Where(ua => ua.CardNumber == cardNumber);
            if (!account.Any()) return null;
            if (account.Count() > 1) throw new InvalidDataException("More than one account with given card number");
            return account.First();
        }

        public void Update(BankAccount ua)
        {
            var current = _userAccounts.Single(_ => _.BankAccountId == ua.BankAccountId);
            current.AccountNumber = ua.AccountNumber;
            current.AccountBalance = ua.AccountBalance;
            current.AuthFailsCount = ua.AuthFailsCount;
            current.CardNumber = ua.CardNumber;
            current.CardPin = ua.CardPin;
            current.FullName = ua.FullName;
            current.IsLocked = ua.IsLocked;
        }

        public BankAccount? Find(long userId) =>
            _userAccounts.SingleOrDefault(_ => _.BankAccountId == userId);
    }
}
