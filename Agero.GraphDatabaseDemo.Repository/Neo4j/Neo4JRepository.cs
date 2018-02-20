using System;
using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using Neo4j.Driver.V1;

namespace Agero.GraphDatabaseDemo.Repository.Neo4j {
	public class Neo4JRepository : IRepository {
		private const int BatchSizeDelete = 10000;

		private const string Movie = Constants.Movie;
		private const string Person = "Person";
		private const string Title = "title";
		private const string Name = "name";

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
					session.Run($"CREATE INDEX ON :{Movie}({Title})");
					session.Run($"CREATE INDEX ON :{Person}({Name})");
				}
			}
		}

		public void CreatePerson(CreatePerson command) {
			RunStatementInTransaction(
				$"CREATE (x:{Person} {CreateProperty(Name, command.Name)}) RETURN x");
		}

		private string CreateProperty(string propertyName, string propertyValue) {
			if (propertyValue == null)
				return string.Empty;

			return $"{{{propertyName}: \"{propertyValue}\"}}";

		}

		public IEnumerable<Person> ListPersons() {
			return List(
				$"MATCH (n:{Person}) RETURN n", "n", PersonFromNode);
		}

		public void CreateMovie(CreateMovie command) {
			RunStatementInTransaction(
				$"CREATE (x:{Movie} {CreateProperty(Title, command.Title)}) RETURN x");
		}

		public IEnumerable<Movie> ListMovies() {
			return List(
				$"MATCH (n:{Movie}) RETURN n", "n", MovieFromNode);
		}

		public void AddActorToMovie(AddActorToMovie command) {
			CreateRelation(command.ActorName, "ACTED_IN", command.MovieTitle);
		}

		public void AddDirectorToMovie(AddDirectorToMovie command) {
			CreateRelation(command.DirectorName, "DIRECTED", command.MovieTitle);
		}

		public IEnumerable<PathNode> ShortestPath(string fromPerson, string toPerson) {
			return ListNodesInPath(
				$"MATCH p=shortestPath((from:{Person} {{{Name}:'{fromPerson}'}})-[*]-(to:{Person} {{{Name}:'{toPerson}'}})) return p", "p", PathNodeFromNode);
		}

		public void Clear() {
			using (var driver = Driver) {
				using (var session = driver.Session()) {
					DeleteNodes(session);
					DeleteIndices(session);
				}
			}
		}

		private void CreateRelation(string personName, string relation, string movieTitle) {
			RunStatementInTransaction(
				$"MATCH(p:{Person} {{ {Name}: '{personName}' }}),(m:{Movie} {{ {Title}: '{movieTitle}' }})\nMERGE(p)-[r:{relation}]->(m)");
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

		private Person PersonFromNode(INode node) {
			return new Person { Name = node.Properties[Name].ToString() };
		}

		private Movie MovieFromNode(INode node) {
			return new Movie { Title = node.Properties[Title].ToString() };
		}

		private PathNode PathNodeFromNode(INode node) {
			return new PathNode { NodeType = PathNodeType(node), NodeInfo = PathNodeInfo(node) };
		}

		private string PathNodeType(INode node) {
			if (node.Labels.Contains(Person))
				return Person;

			if (node.Labels.Contains(Movie))
				return Movie;

			throw new ArgumentException($"Unexpected labels for node: [{string.Join(", ", node.Labels)}]");
		}

		private string PathNodeInfo(INode node) {
			if (node.Labels.Contains(Person))
				return node.Properties[Name].ToString();

			if (node.Labels.Contains(Movie))
				return node.Properties[Title].ToString();

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