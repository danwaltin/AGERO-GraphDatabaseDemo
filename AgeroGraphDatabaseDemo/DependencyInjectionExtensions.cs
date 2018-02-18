using System.Configuration;
using System.Reflection;
using AgeroGraphDatabaseDemo.Controllers;
using AgeroGraphDatabaseDemo.Repository;
using AgeroGraphDatabaseDemo.Repository.Neo4j;
using Autofac;
using Autofac.Integration.WebApi;

namespace AgeroGraphDatabaseDemo {
	public static class DependencyInjectionExtensions {
		public static void RegisterGraphDemoTypes(this ContainerBuilder builder) {
			builder.RegisterApiControllers(WebApiAssembly());

			builder.RegisterType<Neo4JRepository>().As<IPersonRepository>();
			builder.Register(_ => new RepositoryConfiguration {
				Url = ConfigurationManager.AppSettings["url"],
				Username = ConfigurationManager.AppSettings["username"],
				Password = ConfigurationManager.AppSettings["password"]
			}).As<RepositoryConfiguration>();
		}

		private static Assembly WebApiAssembly() => typeof(PersonsController).Assembly;
	}
}