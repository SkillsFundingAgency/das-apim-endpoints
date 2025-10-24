Feature: MissingEarnings

Tests behaviour if matching earnings data is not present

Scenario: Returns empty fm36 block for learner with no earnings data
	Given there is data in learnings domain but no data in earnings domain
	When the FM36 block is retrieved for the mismatched learner
	Then the response should contain an fm36 block with only uln populated
