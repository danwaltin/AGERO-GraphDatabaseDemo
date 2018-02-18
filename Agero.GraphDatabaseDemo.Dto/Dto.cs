namespace Agero.GraphDatabaseDemo.Dto {
	public class Dto {
		public override bool Equals(object obj) {
			return this.EqualProperties(obj);
		}

		public override int GetHashCode() {
			return this.HashOfMembers();
		}

		public override string ToString() {
			return this.DescribeMembers();
		}
	}
}