using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.DAL;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;

namespace GestaoClientesEBeneficiarios.Web
{
    public class DependencyInjectionConfig
    {
        public static void RegisterDIContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<DaoCliente>(Lifestyle.Scoped);
            container.Register<BoCliente>(Lifestyle.Scoped);

            container.Register<DaoBeneficiario>(Lifestyle.Scoped);
            container.Register<BoBeneficiario>(Lifestyle.Scoped);

            container.RegisterSingleton(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(container.GetInstance));
        }
    }
}
