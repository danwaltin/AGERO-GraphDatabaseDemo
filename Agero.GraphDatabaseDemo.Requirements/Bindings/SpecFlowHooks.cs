using Agero.GraphDatabaseDemo.Repository;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings {
	[Binding]
	public class SpecFlowHooks {
		private readonly ScenarioContext _context;

		public SpecFlowHooks(ScenarioContext context) {
			_context = context;
		}

		#region Before/After

		[BeforeScenario]
		public void ClearDatabase() {
			var repository = _context.Resolve<IRepository>();
			repository.Clear();
		}

		#endregion
	}
}