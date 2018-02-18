using System.Collections.Generic;
using AgeroGraphDatabaseDemo.Controllers;
using AgeroGraphDatabaseDemo.Models;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonWriter {
		public void CreatePersons(IEnumerable<CreatePersonRequest> requests) {
			var controller = new PersonsController();
			foreach (var request in requests)
				controller.Create(request);
		}
	}
}