using System.Collections.Generic;
using AgeroGraphDatabaseDemo.Controllers;
using AgeroGraphDatabaseDemo.Models;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonReader {
		public IEnumerable<Person> AllPersons() {
			var controller = new PersonsController();
			return controller.List();
		}
	}
}