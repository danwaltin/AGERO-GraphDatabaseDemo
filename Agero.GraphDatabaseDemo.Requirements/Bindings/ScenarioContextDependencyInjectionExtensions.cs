using Agero.GraphDatabaseDemo.Controllers;
using Agero.GraphDatabaseDemo.Repository.Neo4j;
using Autofac;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings {
	public static class ScenarioContextDependencyInjectionExtensions {
		public static PersonsController PersonsController(this ScenarioContext context) =>
			Resolve<PersonsController>(context);

		public static T Resolve<T>(ScenarioContext context) {
			var container = AutofacContainer(context);
			return container.Resolve<T>();
		}

		private static IContainer AutofacContainer(ScenarioContext context) {
			const string autofacContainer = "autofacContainer";
			if (!context.ContainsKey(autofacContainer))
				context[autofacContainer] = CreateAutofacContainer();

			return (IContainer) context[autofacContainer];
		}

		private static IContainer CreateAutofacContainer() {
			var builder = new ContainerBuilder();

			builder.RegisterGraphDemoTypes();

			// override some registrations
			builder.Register(_ => new RepositoryConfiguration {
				Url = "bolt://localhost:7687",
				Username = "neo4j",
				Password = "agerounittest"
			}).As<RepositoryConfiguration>();

			return builder.Build();
		}
	}
}