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
		public int SixDegreesIndex(string fromPerson, string toPerson) {
			if (fromPerson == toPerson)
				return 0;

			var path = _repository.ShortestPath(fromPerson, toPerson).ToList();
			if (!path.Any())
				return -1;

			return path.Count(node => node.NodeType == Constants.Movie);
		}
	}
}