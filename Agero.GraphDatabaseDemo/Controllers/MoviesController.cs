using System;
using System.Collections.Generic;
using System.Web.Http;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;

namespace Agero.GraphDatabaseDemo.Controllers {
	public class MoviesController : ApiController {
		[HttpPost]
		public IHttpActionResult Create(CreateMovie command) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Movie> List() {
			throw new NotImplementedException();
		}
	}
}