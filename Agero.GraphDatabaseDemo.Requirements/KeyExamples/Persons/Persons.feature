Feature: Persons

Scenario: Adding persons
	When adding the persons
		| Name         |
		| Ada Lovelace |
		| Alan Turing  |
		
	Then the following persons should be available
		| Name         |
		| Ada Lovelace |
		| Alan Turing  |
