using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;
using Agero.TestHelpers;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Persons {
	class PersonAsserter {
		public void AssertPersons(IEnumerable<Person> expected, IEnumerable<Person> actual) {
			new AssertHelper().AssertIEnumerableUnordered(expected, actual);
		}
	}
}