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

		#region Givens, whens and thens

		[When(@"adding the movies")]
		public void WhenAddingTheMovies(Table table) {
			_writer.CreateMovies(
				_mapper.CreateCommands(table));
		}

		[Then(@"the following movies should be available")]
		public void ThenTheFollowingMoviesShouldBeAvailable(Table table) {
			var actual = _reader.AllMovies();
			var expected = _mapper.Persons(table);

			_asserter.AssertMovies(expected, actual);
		}

		#endregion
	}
}