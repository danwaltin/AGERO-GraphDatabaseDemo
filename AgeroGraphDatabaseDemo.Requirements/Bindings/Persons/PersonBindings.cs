using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	[Binding]
	public class PersonBindings {
		private readonly PersonMapper _mapper;
		private readonly PersonWriter _writer;
		private readonly PersonReader _reader;
		private readonly PersonAsserter _asserter;

		public PersonBindings(ScenarioContext context) {
			_mapper = new PersonMapper();
			_writer = new PersonWriter(context);
			_reader = new PersonReader(context);
			_asserter = new PersonAsserter();
		}

		#region Givens, whens and thens

		[When(@"adding the persons")]
		public void WhenAddingThePersons(Table table) {
			_writer.CreatePersons(
				_mapper.CreateRequests(table));
		}

		[Then(@"the following persons should be available")]
		public void ThenTheFollowingPersonsShouldBeAvailable(Table table) {
			var actual = _reader.AllPersons();
			var expected = _mapper.Persons(table);

			_asserter.AssertPersons(expected, actual);
		}

		#endregion
	}
}