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
				return new SixDegrees { Index = 0 };

			var path = _repository.ShortestPath(fromPerson, toPerson).ToList();
			if (!path.Any())
				return new SixDegrees { Index = -1 };

			var index = path.Count(node => node.NodeType == Constants.Movie);
			var p = $"[{string.Join(" -> ", path.Select(node => $"{node.NodeType}({node.NodeInfo})"))}]";
			return new SixDegrees { Index = index , Path =  p};
		}
	}
}