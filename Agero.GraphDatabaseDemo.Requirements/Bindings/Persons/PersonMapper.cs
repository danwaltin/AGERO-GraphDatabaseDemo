﻿using System.Collections.Generic;
using System.Linq;
using Agero.GraphDatabaseDemo.Commands;
using Agero.GraphDatabaseDemo.Dto;
using TechTalk.SpecFlow;

namespace Agero.GraphDatabaseDemo.Requirements.Bindings.Persons {
	public class PersonMapper {
		public IEnumerable<CreatePerson> CreateCommands(Table table) {
			return table.Rows.Select(r => new CreatePerson { Name = r.Name() });
		}

		public IEnumerable<Person> Persons(Table table) {
			return table.Rows.Select(r => new Person { Name = r.Name() });
		}
	}
}