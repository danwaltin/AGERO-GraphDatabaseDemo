using Agero.TestHelpers;
using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	[Binding]
	public class PersonBindings {
		private readonly PersonMapper _mapper;
		private readonly PersonWriter _writer;
		private readonly PersonReader _reader;

		public PersonBindings() {
			_mapper = new PersonMapper();
			_writer = new PersonWriter();
			_reader = new PersonReader();
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

			new AssertHelper().AssertIEnumerableUnordered(expected, actual);
		}

		#endregion
	}
}