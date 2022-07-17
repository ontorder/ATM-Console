using AtmConsole.App;
using AtmConsole.DomainServices.Authentication;
using AtmConsole.Repositories.Transactions;
using AtmConsole.Repositories.UserAccounts;

namespace AtmConsole
{
    internal class Entry
    {
        static void Main()
        {
            var userAccounts = new UserAccountStaticRepository();
            var transactions = new TransactionMemoryRepository();
            var context = new Repositories.AtmContext(transactions, userAccounts);

            var userAuth = new RetryUserAuthentication(userAccounts);
            var cardAuth = new CardAuthentication();

            var atm = new AtmAppService(context);

            var atmApp = new AtmApp(userAuth, cardAuth, atm);
            atmApp.Run();
        }
    }
}
