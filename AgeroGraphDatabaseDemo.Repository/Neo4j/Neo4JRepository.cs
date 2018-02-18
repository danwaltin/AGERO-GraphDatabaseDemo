using System.Collections.Generic;
using Agero.GraphDatabaseDemo.Dto;

namespace AgeroGraphDatabaseDemo.Repository.Neo4j {
	public class Neo4JRepository : IPersonRepository {
		private readonly RepositoryConfiguration _configuration;

		public Neo4JRepository(RepositoryConfiguration configuration) {
			_configuration = configuration;
		}

		public void Create(string name) {
		}

		public IEnumerable<Person> List() {
			return new List<Person>();
		}
	}
}