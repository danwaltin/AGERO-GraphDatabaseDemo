using System.Collections.Generic;
using System.Web.Http;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using Agero.GraphDatabaseDemo.Repository;

namespace Agero.GraphDatabaseDemo.Controllers {
	public class MoviesController : ApiController {
		private readonly IRepository _repository;

		public MoviesController(IRepository repository) {
			_repository = repository;
		}

		[HttpPost]
		public IHttpActionResult Create(CreateMovie command) {
			_repository.CreateMovie(command);
			return Ok();
		}

		[HttpGet]
		public IEnumerable<Movie> List() {
			return _repository.ListMovies();
		}
	}
}