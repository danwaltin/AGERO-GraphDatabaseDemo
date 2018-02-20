using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Persons {
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

		[Given(@"the persons")]
		[When(@"adding the persons")]
		public void CreatePersons(Table table) {
			_writer.CreatePersons(
				_mapper.CreateCommands(table));
		}

		[Then(@"the following persons should be available")]
		public void AssertAvailablePersons(Table table) {
			var actual = _reader.AllPersons();
			var expected = _mapper.Persons(table);

			_asserter.AssertPersons(expected, actual);
		}

		[Then(@"the six degrees index from '(.*)' to '(.*)' is (.*)")]
		public void AsseetSixDegreesIndex(string fromPerson, string toPerson, int expected) {
			var actual = _reader.SixDegreesIndex(fromPerson, toPerson);
			Assert.AreEqual(expected, actual);
		}
	}
}