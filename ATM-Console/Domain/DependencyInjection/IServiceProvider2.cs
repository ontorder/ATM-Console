namespace AtmConsole.Domain.DependencyInjection
{
    public interface IServiceProvider2 : IServiceProvider
    {
        TAbstract? GetService<TAbstract>();
    }
}
