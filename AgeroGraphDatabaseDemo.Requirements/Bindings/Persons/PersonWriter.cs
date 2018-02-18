using System.Collections.Generic;
using AgeroGraphDatabaseDemo.Requests;
using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonWriter {
		private readonly ScenarioContext _context;

		public PersonWriter(ScenarioContext context) {
			_context = context;
		}

		public void CreatePersons(IEnumerable<CreatePersonRequest> requests) {
			var controller = _context.PersonsController();
			foreach (var request in requests)
				controller.Create(request);
		}
	}
}