using System;
using System.Web.Http;
using AgeroGraphDatabaseDemo.Models;

namespace AgeroGraphDatabaseDemo.Controllers {
	public class SixDegreesController : ApiController {
		[HttpGet]
		public Ping Ping() {
			return new Ping {
				Value = "pong",
				Timestamp = DateTime.Now.ToString("s")
			};
		}
	}
}