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

Scenario: Name must be unique
	Given the persons
		| Name         |
		| Ada Lovelace |
		| Alan Turing  |

	When adding the persons
		| Name         |
		| Alan Turing  |

	Then the following persons should be available
		| Name         |
		| Ada Lovelace |
		| Alan Turing  |

Scenario: Name is mandatory
	Given the persons
		| Name         |
		| Ada Lovelace |

	When adding a person without a name

	Then the following persons should be available
		| Name         |
		| Ada Lovelace |
