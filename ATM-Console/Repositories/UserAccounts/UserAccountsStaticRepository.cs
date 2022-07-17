using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Repositories.UserAccounts
{
    public class UserAccountsStaticRepository : IUserAccountsRepository
    {
        private List<UserAccount> _userAccountList;

        public UserAccountsStaticRepository()
        {
            _userAccountList = new List<UserAccount>
            {
                new UserAccount(
                    id: 1,
                    accountBalance: 100000.00m,
                    accountNumber: 543210,
                    cardNumber: 123123,
                    cardPin: 321321,
                    fullName: "Uzumaki Naruto",
                    isLocked: false,
                    totalLogin: 0
                ),

                new UserAccount(
                    id: 2,
                    accountBalance: 40000.00m,
                    accountNumber: 987654,
                    cardNumber: 987987,
                    cardPin: 789789,
                    fullName: "Hatake Kakashi",
                    isLocked: false,
                    totalLogin: 0
                ),

                new UserAccount(
                    id: 3,
                    accountBalance: 150000.00m,
                    accountNumber: 975310,
                    cardNumber: 975975,
                    cardPin: 579579,
                    fullName: "Hyuga Hinata",
                    isLocked: true,
                    totalLogin: 0
                )
            };
        }

        public IEnumerable<UserAccount> Get()
            => _userAccountList;
    }
}
