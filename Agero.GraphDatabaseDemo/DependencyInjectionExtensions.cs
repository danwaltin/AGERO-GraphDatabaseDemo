using System.Configuration;
using System.Reflection;
using Agero.GraphDatabaseDemo.Controllers;
using Agero.GraphDatabaseDemo.Repository;
using Agero.GraphDatabaseDemo.Repository.Neo4j;
using Autofac;
using Autofac.Integration.WebApi;

namespace Agero.GraphDatabaseDemo {
	public static class DependencyInjectionExtensions {
		public static void RegisterGraphDemoTypes(this ContainerBuilder builder) {
			builder.RegisterApiControllers(WebApiAssembly());

			builder.RegisterType<Neo4JRepository>().As<IRepository>();
			builder.Register(_ => new RepositoryConfiguration {
				Url = ConfigurationManager.AppSettings["url"],
				Username = ConfigurationManager.AppSettings["username"],
				Password = ConfigurationManager.AppSettings["password"]
			}).As<RepositoryConfiguration>();
		}

		private static Assembly WebApiAssembly() => typeof(PersonsController).Assembly;
	}
}