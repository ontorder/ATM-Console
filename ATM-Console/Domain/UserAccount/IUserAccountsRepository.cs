namespace AtmConsole.Domain.UserAccounts
{
    public interface IUserAccountsRepository
    {
        IEnumerable<UserAccount> Get();
    }
}
