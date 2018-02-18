using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;

namespace AgeroGraphDatabaseDemo.Repository {
	public interface IPersonRepository {
		void Create(string name);
		IEnumerable<Person> List();
	}
}