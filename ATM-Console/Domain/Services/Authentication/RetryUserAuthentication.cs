using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Domain.Services.Authentication
{
    public class RetryUserAuthentication : IUserAuthentication
    {
        private readonly IUserAccountRepository _users;

        public RetryUserAuthentication(IUserAccountRepository users)
            => _users = users;

        public void UpdateUserAuth(UserAccount ua, bool succeeded)
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
