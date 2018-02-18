using System.Collections.Generic;
using System.Web.Http;
using AgeroGraphDatabaseDemo.Models;

namespace AgeroGraphDatabaseDemo.Controllers {
	public class PersonsController : ApiController {
		[HttpPost]
		public IHttpActionResult Create(CreatePersonRequest request) {
			return Ok();
		}

		[HttpGet]
		public IEnumerable<Person> List() {
			return new List<Person>();
		}
	}
}