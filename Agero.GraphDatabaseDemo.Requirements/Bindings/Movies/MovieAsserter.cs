using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;
using Agero.TestHelpers;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	class MovieAsserter {
		public void AssertMovies(IEnumerable<Movie> expected, IEnumerable<Movie> actual) {
			new AssertHelper().AssertIEnumerableUnordered(expected, actual);
		}
	}
}