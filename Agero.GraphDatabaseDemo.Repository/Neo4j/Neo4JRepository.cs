using System;
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

		public void Initialize() {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					session.Run("CREATE INDEX ON :Movie(title)");
					session.Run("CREATE INDEX ON :Person(name)");
				}
			}
		}

		public void CreatePerson(CreatePerson command) {
			Create($"CREATE (x:Person {{name: \"{command.Name}\"}}) RETURN x");
		}

		public IEnumerable<Person> ListPersons() {
			return List("MATCH (n:Person) RETURN n", "n", Person);
		}

		public void CreateMovie(CreateMovie command) {
			Create($"CREATE (x:Movie {{title: \"{command.Title}\"}}) RETURN x");
		}

		public IEnumerable<Movie> ListMovies() {
			return List("MATCH (n:Movie) RETURN n", "n", Movie);
		}

		public void Clear() {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					DeleteNodes(session);
					DeleteIndices(session);
				}
			}
		}

		private void Create(string statement) {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					using (var transaction = session.BeginTransaction()) {
						transaction.Run(statement);
						transaction.Success();
					}
				}
			}
		}

		private Person Person(INode node) {
			return new Person { Name = node.Properties["name"].ToString() };
		}

		private Movie Movie(INode node) {
			return new Movie { Title = node.Properties["title"].ToString() };
		}

		private IEnumerable<T> List<T>(string statement, string returnKey, Func<INode, T> create) {
			return GetNodes(statement, returnKey).Select(create).ToList();
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