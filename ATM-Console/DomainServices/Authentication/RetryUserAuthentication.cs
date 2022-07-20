using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.DomainServices.Authentication
{
    public class RetryUserAuthentication : IUserAuthentication
    {
        private readonly IBankAccountRepository _users;

        public RetryUserAuthentication(IBankAccountRepository users)
            => _users = users;

        public void UpdateUserAuth(BankAccount ua, bool succeeded)
        {
            if (ua.IsLocked) return;

            if (succeeded)
            {
                ua.AuthFailsCount = 0;
            }
            else
            {
                ++ua.AuthFailsCount;

                if (ua.AuthFailsCount > 3)
                    ua.IsLocked = true;
            }

            _users.Update(ua);
        }
    }
}
