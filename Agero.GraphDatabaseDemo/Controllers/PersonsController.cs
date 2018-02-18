using System.Collections.Generic;
using System.Web.Http;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using Agero.GraphDatabaseDemo.Repository;

namespace Agero.GraphDatabaseDemo.Controllers {
	public class PersonsController : ApiController {
		private readonly IPersonRepository _personRepository;

		public PersonsController(IPersonRepository personRepository) {
			_personRepository = personRepository;
		}

		[HttpPost]
		public IHttpActionResult Create(CreatePerson command) {
			_personRepository.Create(command);
			return Ok();
		}

		[HttpGet]
		public IEnumerable<Person> List() {
			return _personRepository.List();
		}
	}
}