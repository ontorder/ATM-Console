using AtmConsole.App;
using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.DependencyInjection;
using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.UserAccounts;
using AtmConsole.DomainServices.Authentication;
using AtmConsole.Repositories.Transactions;
using AtmConsole.Repositories.UserAccounts;

namespace AtmConsole
{
    internal class Entry
    {
        static void Main()
        {
            IDependencyInjection di = new DomainServices.DependencyInjection.SimpleDependencyInjection();
            di.AddSingleton<IUserAccountRepository, UserAccountStaticRepository>();
            di.AddSingleton<ITransactionRepository, TransactionMemoryRepository>();
            di.AddTransient<Repositories.AtmContext>();
            di.AddTransient<IUserAuthentication, RetryUserAuthentication>();
            di.AddTransient<ICardAuthentication, CardAuthentication>();
            di.AddSingleton<IAtmAppService, AtmAppService>();
            di.AddSingleton<AtmApp>();

            var sp = di.GetServiceProvider();
            var atmApp = sp.GetService<AtmApp>();

            atmApp.Run();
        }
    }
}
