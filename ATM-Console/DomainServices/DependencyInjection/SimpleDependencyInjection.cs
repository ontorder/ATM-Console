using AtmConsole.Domain.DependencyInjection;
using System.Reflection;

namespace AtmConsole.DomainServices.DependencyInjection
{
    public class SimpleDependencyInjection : IDependencyInjection, IServiceProvider2
    {
        private readonly List<(Type AbstractType, Type ConcreteType, DependencyMode Mode)> _dependencies = new();
        private readonly List<(Type AbstractType, object Instance)> _singletons = new();

        public void AddSingleton<TAbstract, TConcrete>() where TConcrete : TAbstract
            => _dependencies.Add((typeof(TAbstract), typeof(TConcrete), DependencyMode.Singleton));

        public void AddSingleton<TObject>(TObject implementation)
        {
            var tobj = typeof(TObject);
            _dependencies.Add((tobj, tobj, DependencyMode.Singleton));
            _singletons.Add((tobj, implementation));
        }

        public void AddTransient<TAbstract, TConcrete>() where TConcrete : TAbstract
            => _dependencies.Add((typeof(TAbstract), typeof(TConcrete), DependencyMode.Transient));

        public object? GetService(Type serviceType)
        {
            var services = _dependencies.Where(dep => dep.AbstractType == serviceType);
            if (services.Any() == false) throw new Exception("no service matches serviceType");
            if (services.Count() > 1) throw new NotSupportedException("this shouldn't happen");
            var selected = services.First();

            if (selected.Mode == DependencyMode.Singleton)
            {
                var filteredSingletons = _singletons.Where(_ => _.AbstractType == serviceType);
                if (filteredSingletons.Any())
                    return filteredSingletons.First().Instance;
            }

            var concreteType = selected.ConcreteType;
            var ctors = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (ctors.Length == 0 || ctors.First().GetParameters().Length == 0)
                return Activator.CreateInstance(concreteType);

            foreach (var ctor in ctors.OrderBy(ctor => ctor.GetParameters().Length))
            {
                var pars = new List<object>();
                foreach (var param in ctor.GetParameters())
                {
                    var argService = GetService(param.ParameterType);
                    if (argService == null) throw new Exception("service not registered");
                    pars.Add(argService);
                }
                var instance = ctor.Invoke(pars.ToArray());
                if (selected.Mode == DependencyMode.Singleton)
                    _singletons.Add((selected.AbstractType, instance));
                return instance;
            }
            throw new Exception("cannot construct service");
        }

        public IServiceProvider2 GetServiceProvider()
            => this;

        public TAbstract? GetService<TAbstract>()
            => (TAbstract)GetService(typeof(TAbstract));

        public void AddTransient<TConcrete>()
            => _dependencies.Add((typeof(TConcrete), typeof(TConcrete), DependencyMode.Transient));

        public void AddSingleton<TConcrete>()
            => _dependencies.Add((typeof(TConcrete), typeof(TConcrete), DependencyMode.Singleton));
    }
}
