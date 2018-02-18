using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;

namespace Agero.GraphDatabaseDemo.Repository {
	public interface IRepository {
		void CreatePerson(CreatePerson command);
		IEnumerable<Person> ListPersons();

		void Clear();
	}
}