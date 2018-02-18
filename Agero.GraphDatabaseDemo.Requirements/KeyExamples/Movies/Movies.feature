Feature: Movies

Scenario: Adding movies
	When adding the movies
		| Title               |
		| Flåklypa            |
		| Alice i underlandet |
		
	Then the following movies should be available
		| Title               |
		| Flåklypa            |
		| Alice i underlandet |
