using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using Neo4j.Driver.V1;

namespace Agero.GraphDatabaseDemo.Repository.Neo4j {
	public class Neo4JRepository : IRepository {
		private const int BatchSizeDelete = 10000;

		private readonly RepositoryConfiguration _configuration;

		public Neo4JRepository(RepositoryConfiguration configuration) {
			_configuration = configuration;
		}

		private IDriver Driver =>
			GraphDatabase.Driver(
				_configuration.Url,
				AuthTokens.Basic(_configuration.Username, _configuration.Password));

		public void CreatePerson(CreatePerson command) {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					using (var transaction = session.BeginTransaction()) {
						var statement = $"CREATE (x:Person {{name: \"{command.Name}\"}}) RETURN x";
						transaction.Run(statement);
						transaction.Success();
					}
				}
			}
		}

		public IEnumerable<Person> ListPersons() {
			var statement = $"MATCH (n:Person) RETURN n";
			const string returnKey = "n";
			return GetNodes(statement, returnKey).Select(node => new Person { Name = node.Properties["name"].ToString() }).ToList();
		}

		public void Clear() {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					DeleteNodes(session);
					DeleteIndices(session);
				}
			}
		}

		public IReadOnlyList<INode> GetNodes(string statement, string returnKey) {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					var result = session.Run(statement);
					return result.Select(record => record[returnKey].As<INode>()).ToList();
				}
			}
		}

		private void DeleteNodes(IStatementRunner runner) {
			int deletedNodes;
			do {
				var statement = $"MATCH (n) WITH n LIMIT {BatchSizeDelete} DETACH DELETE n";
				var result = runner.Run(statement);
				deletedNodes = result.Summary.Counters.NodesDeleted;
			} while (deletedNodes == BatchSizeDelete);
		}

		private void DeleteIndices(IStatementRunner runner) {
			var indexResult = runner.Run("CALL db.indexes()");
			using (var enumerator = indexResult.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					var record = enumerator.Current;
					if (record != null) {
						// Example: DROP INDEX ON: Person(personId)
						var statement = $"DROP {record.Values["description"]} ";
						runner.Run(statement);
					}
				}
			}
		}
	}
}