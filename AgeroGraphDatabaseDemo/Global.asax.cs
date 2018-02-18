using System.Web.Http;

namespace AgeroGraphDatabaseDemo {
	public class WebApiApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}