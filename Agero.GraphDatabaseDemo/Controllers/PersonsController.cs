using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using Agero.GraphDatabaseDemo.Repository;

namespace Agero.GraphDatabaseDemo.Controllers {
	public class PersonsController : ApiController {
		private readonly IRepository _repository;

		public PersonsController(IRepository repository) {
			_repository = repository;
		}

		[HttpPost]
		public IHttpActionResult Create(CreatePerson command) {
			_repository.CreatePerson(command);
			return Ok();
		}

		[HttpGet]
		public IEnumerable<Person> List() {
			return _repository.ListPersons();
		}

		[HttpGet]
		public SixDegrees SixDegreesIndex(string fromPerson, string toPerson) {
			if (fromPerson == toPerson)
				return SamePerson();

			var nodes = _repository.ShortestPath(fromPerson, toPerson).ToList();
			if (!nodes.Any())
				return NoPathFound();

			return new SixDegrees {
				Index = Index(nodes),
				Path = Path(nodes)
			};
		}

		private int Index(IEnumerable<PathNode> nodes) {
			return nodes.Count(node => node.NodeType == Constants.Movie);
		}

		private string Path(IEnumerable<PathNode> nodes) {
			return $"[{string.Join(" -> ", nodes.Select(node => $"{node.NodeType}({node.NodeInfo})"))}]";
		}

		private SixDegrees SamePerson() {
			return new SixDegrees { Index = 0 };
		}

		private SixDegrees NoPathFound() {
			return new SixDegrees { Index = -1 };
		}
	}
}