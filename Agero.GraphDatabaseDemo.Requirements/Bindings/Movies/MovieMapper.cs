using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Movies {
	public class MovieMapper {
		public IEnumerable<CreateMovie> CreateCommands(Table table) {
			return table.Rows.Select(r => new CreateMovie { Title = r.Title() });
		}

		public IEnumerable<AddActorToMovie> AddActorToMovieCommands(string movieTitle, Table table) {
			return table.Rows.Select(r => new AddActorToMovie { ActorName = r.Name(), MovieTitle = movieTitle });
		}

		public IEnumerable<Movie> Persons(Table table) {
			return table.Rows.Select(r => new Movie { Title = r.Title() });
		}
	}
}