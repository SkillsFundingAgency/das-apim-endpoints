Feature: MissingEarnings

Tests behaviour if matching learning data is not present for the earnings returned (the feature name is historical; earnings are now retrieved first)

Scenario: Returns 400 Bad Request when learning data is missing for the earnings returned
	Given there is data in earnings domain but no data in learnings domain
	When the FM36 block is retrieved for the mismatched learner
	Then the response should be a 400 Bad Request
