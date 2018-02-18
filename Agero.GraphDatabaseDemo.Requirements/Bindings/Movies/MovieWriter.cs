using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	class MovieWriter {
		private readonly ScenarioContext _context;

		public MovieWriter(ScenarioContext context) {
			_context = context;
		}

		public void CreateMovies(IEnumerable<CreateMovie> commands) {
			var controller = _context.MoviesController();
			foreach (var command in commands)
				controller.Create(command);
		}
	}
}