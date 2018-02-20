using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;

namespace Agero.GraphDatabaseDemo.Repository {
	public interface IRepository {
		void Initialize();

		void CreatePerson(CreatePerson command);
		IEnumerable<Person> ListPersons();

		void CreateMovie(CreateMovie command);
		IEnumerable<Movie> ListMovies();

		void AddActorToMovie(AddActorToMovie command);

		IEnumerable<PathNode> ShortestPath(string fromPerson, string toPerson);

		void Clear();
	}
}