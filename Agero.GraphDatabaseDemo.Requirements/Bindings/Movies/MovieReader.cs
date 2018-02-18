using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	class MovieReader {
		private readonly ScenarioContext _context;

		public MovieReader(ScenarioContext context) {
			_context = context;
		}

		public IEnumerable<Movie> AllMovies() {
			var controller = _context.MoviesController();
			return controller.List();
		}
	}
}