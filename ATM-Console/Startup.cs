using AtmConsole.App;
using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.DependencyInjection;
using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.BankAccounts;
using AtmConsole.DomainServices.Authentication;
using AtmConsole.Repositories.Transactions;
using AtmConsole.Repositories.UserAccounts;

namespace AtmConsole
{
    // isolate ui from app/api?
    //    non per forza per avere livello controller ecc, però
    //    potrebbe tornare utile per bindare ui->appsvc, eventi->ui
    // should be repositories->context, not context->repositories
    // change userAccount to user->card, user->bankAccount, card->bankAccount?
    public class Startup
    {
        IDependencyInjection _services;

        public Startup()
        {
            _services = new DomainServices.DependencyInjection.SimpleDependencyInjection();
            SetupData();
            SetupAtuthentication();
            _services.AddSingleton<IAtmAppService, AtmAppService>();
            _services.AddSingleton<AtmApp>();
        }

        public void Start()
        {
            var provider = _services.GetServiceProvider();
            var atmApp = provider.GetService<AtmApp>();
            if (atmApp == null) throw new InvalidOperationException("oops");
            atmApp.Run();
        }

        private void SetupData()
        {
            _services.AddSingleton<IBankAccountRepository, UserAccountStaticRepository>();
            _services.AddSingleton<ITransactionRepository, TransactionMemoryRepository>();
            _services.AddTransient<Repositories.AtmContext>();
        }

        private void SetupAtuthentication()
        {
            _services.AddTransient<IUserAuthentication, RetryUserAuthentication>();
            _services.AddTransient<ICardAuthentication, CardAuthentication>();
        }
    }
}
