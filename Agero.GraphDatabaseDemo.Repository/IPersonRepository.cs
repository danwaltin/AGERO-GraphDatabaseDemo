using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;

namespace Agero.GraphDatabaseDemo.Repository {
	public interface IPersonRepository {
		void Create(CreatePerson command);
		IEnumerable<Person> List();
	}
}