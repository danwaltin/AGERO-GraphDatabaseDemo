using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;

namespace Agero.GraphDatabaseDemo.Repository.Neo4j {
	public class Neo4JRepository : IPersonRepository {
		private readonly RepositoryConfiguration _configuration;

		public Neo4JRepository(RepositoryConfiguration configuration) {
			_configuration = configuration;
		}

		public void Create(CreatePerson command) {
		}

		public IEnumerable<Person> List() {
			return new List<Person>();
		}
	}
}