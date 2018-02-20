Feature: Six Degrees Actors and Directors

Background: 
	Given the movies
		| Title                          |
		| The Fellowship of the Ring     |
		| King Kong                      |
		| En hittepåfilm                 |
		| En hittepåfilm, andra sommaren |
	And the persons
		| Name              |
		| Cate Blanchett    |
		| Peter Jackson     |
		| Jane Doe          |
		| Naomi Watts       |

Scenario Outline: The six degrees index is one between an actor and the director of a movie
	When the movie 'The Fellowship of the Ring' has the director 'Peter Jackson'
	And the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Cate Blanchett | Peter Jackson  | 1                 |
		| Peter Jackson  | Cate Blanchett | 1                 |

Scenario Outline: The shortest path includes directors
	When the movie 'The Fellowship of the Ring' has the director 'Peter Jackson'
	And the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
	
	And the movie 'King Kong' has the director 'Peter Jackson'
	And the movie 'King Kong' has the actors
		| Name        |
		| Naomi Watts |

	And the movie 'En hittepåfilm' has the actors
		| Name           |
		| Cate Blanchett |
		| Jane Doe       |
	And the movie 'En hittepåfilm, andra sommaren' has the actors
		| Name        |
		| Jane Doe    |
		| Naomi Watts |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Cate Blanchett | Naomi Watts    | 2                 |
		| Naomi Watts    | Cate Blanchett | 2                 |
