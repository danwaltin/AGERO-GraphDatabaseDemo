using AgeroGraphDatabaseDemo.Controllers;
using Autofac;
using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings {
	public static class ScenarionContextDependencyInjectionExtensions {
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

			return builder.Build();
		}
	}
}