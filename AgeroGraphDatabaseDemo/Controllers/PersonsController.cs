using System.Collections.Generic;
using System.Web.Http;
using Agero.GraphDatabaseDemo.Dto;
using AgeroGraphDatabaseDemo.Repository;
using AgeroGraphDatabaseDemo.Requests;

namespace AgeroGraphDatabaseDemo.Controllers {
	public class PersonsController : ApiController {
		private readonly IPersonRepository _personRepository;

		public PersonsController(IPersonRepository personRepository) {
			_personRepository = personRepository;
		}

		[HttpPost]
		public IHttpActionResult Create(CreatePersonRequest request) {
			_personRepository.Create(request.Name);
			return Ok();
		}

		[HttpGet]
		public IEnumerable<Person> List() {
			return _personRepository.List();
		}
	}
}