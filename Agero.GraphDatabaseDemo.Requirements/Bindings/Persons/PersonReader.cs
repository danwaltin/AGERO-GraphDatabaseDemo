using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Controllers;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonReader {
		private readonly ScenarioContext _context;

		public PersonReader(ScenarioContext context) {
			_context = context;
		}

		public IEnumerable<Person> AllPersons() {
			return PersonsController().List();
		}

		public int SixDegreesIndex(string fromPerson, string toPerson) {
			return PersonsController().SixDegreesIndex(fromPerson, toPerson);
		}

		private PersonsController PersonsController() {
			return _context.PersonsController();
		}
	}
}