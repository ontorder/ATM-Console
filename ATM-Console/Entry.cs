using AtmConsole.App;
using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.Services.Authentication;
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
            var context = new Repositories.AtmConsoleContext(transactions, userAccounts);

            var userAuth = new RetryUserAuthentication(userAccounts);
            var cardAuth = new CardAuthentication();

            var atmApp = new AtmApp(context, userAuth, cardAuth);
            atmApp.Run();
        }
    }
}
