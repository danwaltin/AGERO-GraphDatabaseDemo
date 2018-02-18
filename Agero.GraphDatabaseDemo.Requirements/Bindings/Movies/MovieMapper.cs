using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	public class MovieMapper {
		public IEnumerable<CreateMovie> CreateCommands(Table table) {
			return table.Rows.Select(r => new CreateMovie { Title = Title(r) });
		}

		public IEnumerable<Movie> Persons(Table table) {
			return table.Rows.Select(r => new Movie { Title = Title(r) });
		}

		private static string Title(TableRow row) {
			return row["Title"];
		}
	}
}