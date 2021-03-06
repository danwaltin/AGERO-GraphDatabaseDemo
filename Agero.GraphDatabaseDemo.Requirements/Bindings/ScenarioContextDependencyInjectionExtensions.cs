﻿using Agero.GraphDatabaseDemo.Controllers;
using Agero.GraphDatabaseDemo.Repository.Neo4j;
using Autofac;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings {
	public static class ScenarioContextDependencyInjectionExtensions {
		public static PersonsController PersonsController(this ScenarioContext context) =>
			context.Resolve<PersonsController>();

		public static MoviesController MoviesController(this ScenarioContext context) =>
			context.Resolve<MoviesController>();

		public static T Resolve<T>(this ScenarioContext context) {
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
				Password = "agero"
			}).As<RepositoryConfiguration>();

			return builder.Build();
		}
	}
}