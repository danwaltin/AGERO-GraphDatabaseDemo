using Agero.GraphDatabaseDemo.Commands;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	[Binding]
	public class MovieBindings {
		private readonly MovieMapper _mapper;
		private readonly MovieWriter _writer;
		private readonly MovieReader _reader;
		private readonly MovieAsserter _asserter;

		public MovieBindings(ScenarioContext context) {
			_mapper = new MovieMapper();
			_writer = new MovieWriter(context);
			_reader = new MovieReader(context);
			_asserter = new MovieAsserter();
		}

		[Given(@"the movies")]
		[When(@"adding the movies")]
		public void CreateMovies(Table table) {
			_writer.CreateMovies(
				_mapper.CreateCommands(table));
		}

		[When(@"the movie '(.*)' has the actors")]
		public void AddActorsToMovie(string movieTitle, Table table) {
			_writer.AddActorsToMovies(
				_mapper.AddActorToMovieCommands(movieTitle, table));
		}

		[Then(@"the following movies should be available")]
		public void AssertAvailableMovies(Table table) {
			var actual = _reader.AllMovies();
			var expected = _mapper.Persons(table);

			_asserter.AssertMovies(expected, actual);
		}

		[When(@"the movie '(.*)' has the director '(.*)'")]
		public void AddDirectorToMovie(string movieTitle, string directorName) {
			_writer.AddDirectorToMovie(
				_mapper.AddDirectorToMovieCommand(movieTitle, directorName ));
		}
	}
}