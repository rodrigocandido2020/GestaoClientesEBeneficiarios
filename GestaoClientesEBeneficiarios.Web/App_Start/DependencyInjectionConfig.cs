using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.DAL;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;

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
            // Registra as classes com seus estilos de vida
            container.Register<DaoBeneficiario>(Lifestyle.Scoped);
            container.Register<BoBeneficiario>(Lifestyle.Scoped);
            //container.Register<DaoBeneficiario>(Lifestyle.Scoped);

            // Opcional: registre via interface se houver
            // container.Register<IDaoBeneficiario, DaoBeneficiario>(Lifestyle.Scoped);
        }
    }
}
