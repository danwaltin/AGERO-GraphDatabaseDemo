using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AgeroGraphDatabaseDemo {
	public static class ObjectExtensions {
		public static bool EqualProperties<T>(this T receiver, object other, bool includeId = true) {
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(receiver, other))
				return true;

			var type = receiver.GetType();

			if (other.GetType() != type)
				return false;

			if (!receiver.Equal(type.GetProperties(), other, Value, includeId))
				return false;

			if (!receiver.Equal(type.GetFields(), other, Value, includeId))
				return false;

			return true;
		}

		private static bool Equal<T>(this object receiver, IEnumerable<T> members, object other, MemberValue<T> memberValue, bool includeId) where T : MemberInfo {
			foreach (var f in members)
				if (!f.Equal(memberValue, receiver, other, includeId))
					return false;

			return true;
		}

		private static bool Equal<T>(this T field, MemberValue<T> memberValue, object receiver, object other, bool includeId) where T : MemberInfo {
			if (field.Name == "Id" && !includeId)
				return true;

			var receiverValue = memberValue(field, receiver);
			var otherValue = memberValue(field, other);

			return EqualValues(receiverValue, otherValue);
		}

		private static bool EqualValues(object receiverValue, object otherValue) {
			if (ReferenceEquals(null, receiverValue))
				return ReferenceEquals(null, otherValue);

			if (IsIList(receiverValue.GetType()) && otherValue != null && IsIList(otherValue.GetType()))
				return EqualIList((IList) receiverValue, (IList) otherValue);

			return receiverValue.Equals(otherValue);
		}

		private static bool IsIList(Type type) {
			foreach (var i in type.GetInterfaces())
				if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>))
					return true;

			return false;
		}

		private static bool EqualIList(IList receiverValue, IList otherValue) {
			if (ReferenceEquals(null, otherValue))
				return false;

			if (receiverValue.Count != otherValue.Count)
				return false;

			for (var index = 0; index < receiverValue.Count; index++)
				if (!receiverValue[index].Equals(otherValue[index]))
					return false;

			return true;
		}

		public static int HashOfMembers(this object receiver) {
			var type = receiver.GetType();

			unchecked // Overflow is fine, just wrap
			{
				var hash = receiver.Hash(type.GetProperties(), Value);
				hash += receiver.Hash(type.GetFields(), Value);

				return hash;
			}
		}

		private static int Hash<T>(this object receiver, IEnumerable<T> members, MemberValue<T> memberValue)
			where T : MemberInfo {
			unchecked // Overflow is fine, just wrap
			{
				var hash = 17;
				foreach (var p in members) {
					var propertyValue = memberValue(p, receiver);
					hash = hash * 23 + (propertyValue?.GetHashCode() ?? 0);
				}
				return hash;
			}
		}

		public static string DescribeMembers(this object receiver) {
			var type = receiver.GetType();

			var values = new List<string>();
			values.AddRange(receiver.MemberValues(type.GetFields(), Value));
			values.AddRange(receiver.MemberValues(type.GetProperties(), Value));

			return $"{type.Name}({string.Join(", ", values.ToArray())})";
		}

		private static IEnumerable<string> MemberValues<T>(
			this object receiver,
			IEnumerable<T> members,
			MemberValue<T> memberValue) where T : MemberInfo {
			var values = new List<string>();
			foreach (var f in members) {
				var value = memberValue(f, receiver);
				var valueDescription = value;

				if (value == null)
					valueDescription = "<null>";
				else if (IsIList(value.GetType()))
					valueDescription = DescribeIList((IList) value);

				values.Add(item: string.Format("{0}={1}", f.Name, valueDescription));
			}
			return values;
		}

		private static string DescribeIList(IList list) {
			var listValues = new List<string>();
			foreach (var value in list)
				listValues.Add(value.ToString());

			return string.Format("[{0}]", string.Join(", ", listValues.ToArray()));
		}

		private static object Value(PropertyInfo p, object receiver) {
			return p.GetValue(receiver, new object[] { });
		}

		private static object Value(FieldInfo f, object receiver) {
			return f.GetValue(receiver);
		}

		private delegate object MemberValue<in T>(T member, object receiver) where T : MemberInfo;
	}
}