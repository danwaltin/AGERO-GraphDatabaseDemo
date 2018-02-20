using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Controllers;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	class MovieWriter {
		private readonly ScenarioContext _context;

		public MovieWriter(ScenarioContext context) {
			_context = context;
		}

		public void CreateMovies(IEnumerable<CreateMovie> commands) {
			foreach (var command in commands)
				MoviesController().Create(command);
		}

		public void AddActorsToMovies(IEnumerable<AddActorToMovie> commands) {
			foreach (var command in commands)
				MoviesController().AddActorToMovie(command);
		}

		private MoviesController MoviesController() {
			return _context.MoviesController();
		}
	}
}