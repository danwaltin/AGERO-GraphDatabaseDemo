using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Dto;
using AgeroGraphDatabaseDemo.Requests;
using TechTalk.SpecFlow;

namespace AgeroGraphDatabaseDemo.Requirements.Bindings.Persons {
	public class PersonMapper {
		public IEnumerable<CreatePersonRequest> CreateRequests(Table table) {
			return table.Rows.Select(r => new CreatePersonRequest { Name = Name(r) });
		}

		public IEnumerable<Person> Persons(Table table) {
			return table.Rows.Select(r => new Person { Name = Name(r) });
		}

		private static string Name(TableRow row) {
			return row["Name"];
		}
	}
}