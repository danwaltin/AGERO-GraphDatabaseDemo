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
			RunStatementInTransaction(
				$"CREATE (x:Person {{name: \"{command.Name}\"}}) RETURN x");
		}

		public IEnumerable<Person> ListPersons() {
			return List(
				"MATCH (n:Person) RETURN n", "n", Person);
		}

		public void CreateMovie(CreateMovie command) {
			RunStatementInTransaction(
				$"CREATE (x:Movie {{title: \"{command.Title}\"}}) RETURN x");
		}

		public IEnumerable<Movie> ListMovies() {
			return List(
				"MATCH (n:Movie) RETURN n", "n", Movie);
		}

		public void AddActorToMovie(AddActorToMovie command) {
			RunStatementInTransaction(
				$"MATCH(p:Person {{ name: '{command.ActorName}' }}),(m:Movie {{ title: '{command.MovieTitle}' }})\nMERGE(p) -[r:ACTED_IN]->(m)");
		}

		public IEnumerable<PathNode> ShortestPath(string fromPerson, string toPerson) {
			return ListNodesInPath(
				$"MATCH p=shortestPath((from:Person {{name:'{fromPerson}'}})-[*]-(to:Person {{name:'{toPerson}'}})) return p", "p", PathNode);
		}

		public void Clear() {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					DeleteNodes(session);
					DeleteIndices(session);
				}
			}
		}

		private void RunStatementInTransaction(string statement) {
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

		private PathNode PathNode(INode node) {
			return new PathNode { NodeType = PathNodeType(node), NodeInfo = PathNodeInfo(node) };
		}

		private string PathNodeType(INode node) {
			if (node.Labels.Contains("Person"))
				return "Person";

			if (node.Labels.Contains("Movie"))
				return "Movie";

			throw new ArgumentException($"Unexpected labels for node: [{string.Join(", ", node.Labels)}]");
		}

		private string PathNodeInfo(INode node) {
			if (node.Labels.Contains("Person"))
				return node.Properties["name"].ToString();

			if (node.Labels.Contains("Movie"))
				return node.Properties["title"].ToString();

			throw new ArgumentException($"Unexpected labels for node: [{string.Join(", ", node.Labels)}]");
		}

		private IEnumerable<T> List<T>(string statement, string returnKey, Func<INode, T> create) {
			return List<INode>(statement, returnKey).Select(create).ToList();
		}

		private IEnumerable<T> ListNodesInPath<T>(string statement, string returnKey, Func<INode, T> create) {
			return GetNodesInPath(statement, returnKey).Select(create).ToList();
		}

		public IReadOnlyList<T> List<T>(string statement, string returnKey) {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					var result = session.Run(statement);

					return result.Select(record => record[returnKey].As<T>()).ToList();
				}
			}
		}

		public IReadOnlyList<INode> GetNodesInPath(string statement, string returnKey) {
			var paths = List<IPath>(statement, returnKey);
			return paths.SelectMany(p => p.Nodes).ToList();
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