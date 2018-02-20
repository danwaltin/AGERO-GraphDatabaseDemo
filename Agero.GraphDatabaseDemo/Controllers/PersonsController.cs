using System.Collections.Generic;
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
			throw new System.NotImplementedException();
		}
	}
}