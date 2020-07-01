using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Model;

namespace ImTech.Service
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDataBaseService, DataBaseService>();
            container.RegisterType<ILogger, Logger>();

            var dataServices = container.Resolve<IDataBaseService>();

            container.RegisterInstance(new StaticDataServices(dataServices));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}