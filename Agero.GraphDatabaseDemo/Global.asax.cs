using System.Web.Http;
using Agero.GraphDatabaseDemo.Repository;
using Autofac;
using Autofac.Integration.WebApi;

namespace Agero.GraphDatabaseDemo {
	public class WebApiApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			GlobalConfiguration.Configure(WebApiConfig.Register);

			SetupDependencyInjection();

			InitializeRepository();
		}

		private void InitializeRepository() {
			var config = GlobalConfiguration.Configuration;
			var repository = (IRepository) config.DependencyResolver.GetService(typeof(IRepository));
			repository.Initialize();
		}

		private void SetupDependencyInjection() {
			var builder = new ContainerBuilder();

			builder.RegisterGraphDemoTypes();

			var container = builder.Build();
			var config = GlobalConfiguration.Configuration;
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
	}
}