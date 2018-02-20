using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings {
	public static class TableRowExtensions {
		public static string Title(this TableRow row) {
			return row["Title"];
		}

		public static string Name(this TableRow row) {
			return row["Name"];
		}
	}
}