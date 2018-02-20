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
			foreach (var command in commands)
				PersonsController().Create(command);
		}

		public void CreatePersonWithName(string name) {
			PersonsController().Create(new CreatePerson { Name = name });
		}

		private Controllers.PersonsController PersonsController() {
			return _context.PersonsController();
		}
	}
}