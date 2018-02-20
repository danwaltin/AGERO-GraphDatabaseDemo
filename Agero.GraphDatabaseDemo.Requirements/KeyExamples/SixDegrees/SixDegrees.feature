Feature: Six Degrees

Background: 
	Given the movies
		| Title                          |
		| The Fellowship of the Ring     |
		| Elisabeth the Golden Age       |
		| Mamma Mia!                     |
		| En hittepåfilm                 |
		| En hittepåfilm, andra sommaren |
	And the persons
		| Name              |
		| Cate Blanchett    |
		| Liv Tyler         |
		| Rhys Ifans        |
		| Stellan Skarsgård |
		| Julie Walters     |
		| Adam Godly        |
		| Jane Doe          |

Scenario Outline: The six degrees index is one for actors in the same movie
	When the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
		| Liv Tyler      |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Cate Blanchett | Liv Tyler      | 1                 |
		| Liv Tyler      | Cate Blanchett | 1                 |

Scenario Outline: The six degrees index is two for actors in different movies with a common actor
	When the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
		| Liv Tyler      |
	And the movie 'Elisabeth the Golden Age' has the actors
		| Name           |
		| Cate Blanchett |
		| Rhys Ifans     |
		| Adam Godly     |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Liv Tyler      | Rhys Ifans     | 2                 |
		| Rhys Ifans     | Liv Tyler      | 2                 |

Scenario Outline: The shortest path is used if there are more than one connection
	When the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
		| Liv Tyler      |
	And the movie 'Elisabeth the Golden Age' has the actors
		| Name           |
		| Cate Blanchett |
		| Rhys Ifans     |
		| Adam Godly     |
	And the movie 'En hittepåfilm' has the actors
		| Name       |
		| Adam Godly |
		| Jane Doe   |
	And the movie 'En hittepåfilm, andra sommaren' has the actors
		| Name      |
		| Jane Doe  |
		| Liv Tyler |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Cate Blanchett | Liv Tyler      | 1                 |
		| Liv Tyler      | Cate Blanchett | 1                 |

Scenario Outline: The six degrees index is zero for an actor to him or herself
	When the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
		| Liv Tyler      |
	And the movie 'Elisabeth the Golden Age' has the actors
		| Name           |
		| Cate Blanchett |
		| Rhys Ifans     |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person      | six degrees index |
		| Cate Blanchett | Cate Blanchett | 0                 |
		| Liv Tyler      | Liv Tyler      | 0                 |

Scenario Outline: The six degrees index is minus one for actors without a connection
	When the movie 'The Fellowship of the Ring' has the actors
		| Name           |
		| Cate Blanchett |
		| Liv Tyler      |
	And the movie 'Mamma Mia!' has the actors
		| Name              |
		| Stellan Skarsgård |
		| Julie Walters     |

	Then the six degrees index from '<from person>' to '<to person>' is <six degrees index>

	Examples:
		| from person    | to person         | six degrees index |
		| Cate Blanchett | Stellan Skarsgård | -1                |
