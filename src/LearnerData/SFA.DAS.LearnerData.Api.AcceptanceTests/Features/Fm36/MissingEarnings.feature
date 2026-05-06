Feature: MissingEarnings

Tests behaviour if matching earnings data is not present

Scenario: Returns 400 Bad Request when earnings data is missing
	Given there is data in learnings domain but no data in earnings domain
	When the FM36 block is retrieved for the mismatched learner
	Then the response should be a 400 Bad Request
