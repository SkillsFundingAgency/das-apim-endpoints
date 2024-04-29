Feature: GetApprentice
	As the portal APPLICATION
	I need to retrieve my apprentice details

Scenario: The apprentice exists
	Given there is an apprentice
	When the apprentice is requested
	Then the result should contain the apprentice data

Scenario: The apprentice does not exists
	Given there is no apprentice
	When the apprentice is requested
	Then the result should be NotFound