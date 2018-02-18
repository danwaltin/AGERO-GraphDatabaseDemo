using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonWriter {
		private readonly ScenarioContext _context;

		public PersonWriter(ScenarioContext context) {
			_context = context;
		}

		public void CreatePersons(IEnumerable<CreatePerson> commands) {
			var controller = _context.PersonsController();
			foreach (var command in commands)
				controller.Create(command);
		}
	}
}