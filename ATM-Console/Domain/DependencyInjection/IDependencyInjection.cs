namespace AtmConsole.Domain.DependencyInjection
{
    public interface IDependencyInjection
    {
        void AddTransient<TAbstract, IConcrete>() where IConcrete : TAbstract;
        void AddTransient<TConcrete>();
        void AddSingleton<TAbstract, TConcrete>() where TConcrete : TAbstract;
        void AddSingleton<TObject>(TObject implementation);
        void AddSingleton<TConcrete>();
        IServiceProvider2 GetServiceProvider();
    }
}
