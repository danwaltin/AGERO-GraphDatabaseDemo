using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonReader {
		private readonly ScenarioContext _context;

		public PersonReader(ScenarioContext context) {
			_context = context;
		}

		public IEnumerable<Person> AllPersons() {
			var controller = _context.PersonsController();
			return controller.List();
		}
	}
}