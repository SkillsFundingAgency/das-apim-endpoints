Feature: GetApprenticeships
	As an apprentice
	I need to have a start screen showing all the steps of my commitment statement
	So that I know what steps are involved in signing my commitment statement
	And I can be directed to sign every aspect of my statement

Scenario: List apprenticeships
	Given there is an apprenticeship
	When the list of apprenticeships is requested
	Then the result should be OK
	And the result should contain the apprenticeship

Scenario: Apprenticeship overview
	Given there is an apprenticeship
	When the apprenticeship overview is requested
	Then the result should be OK
	And the apprenticeship is returned

Scenario: No current apprenticeship
	Given there is no apprenticeship
	When the apprenticeship overview is requested
	Then the result should be NotFound

