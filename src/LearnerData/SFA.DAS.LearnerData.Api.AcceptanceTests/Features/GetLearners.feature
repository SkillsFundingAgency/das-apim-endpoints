Feature: GetLearners

These tests confirm the functionality of the GetLearners endpoint

Scenario: Returns first page of a list of learners
	Given there are 100 learners in the system
	When I request the first page of learners
	Then I should receive a response with 20 learners
	And the response has headers for the next page only
	And the reponse contains the following information
		| Total | Page | PageSize | TotalPages |
		| 100   | 1    | 20       | 5          |

Scenario: Returns second page of a list of learners
	Given there are 100 learners in the system
	When I request the second page of learners
	Then I should receive a response with 20 learners
	And the response has headers for both next and previous pages
	And the reponse contains the following information
		| Total | Page | PageSize | TotalPages |
		| 100   | 2    | 20       | 5          |

Scenario: Returns last page of a list of learners
	Given there are 100 learners in the system
	When I request the fifth page of learners
	Then I should receive a response with 20 learners
	And the response has headers for the previous page only
	And the reponse contains the following information
		| Total | Page | PageSize | TotalPages |
		| 100   | 5    | 20       | 5          |

Scenario: Returns empty list when no learners exist
	Given there are no learners in the system
	When I request the first page of learners
	Then I should receive a response with 0 learners
	And the response has no headers for next or previous pages
	And the reponse contains the following information
		| Total | Page | PageSize | TotalPages |
		| 0     | 1    | 20       | 0          |

Scenario: Returns learners with specified page size
	Given there are 100 learners in the system
	When I request the first page of learners with a page size of 10
	Then I should receive a response with 10 learners
	And the response has headers for the next page only
	And the reponse contains the following information
		| Total | Page | PageSize | TotalPages |
		| 100   | 1    | 10       | 10         |