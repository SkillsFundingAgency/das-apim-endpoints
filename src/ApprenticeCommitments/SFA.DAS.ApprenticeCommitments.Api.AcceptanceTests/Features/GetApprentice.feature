Feature: GetApprentice
	As an apprentice
	I need to retrieve my details

Scenario: The apprentice exists
	Given there is an apprentice
	When the apprentice is requested
	Then the result should contain the apprentice data

Scenario: The apprenticeship does not exist
	Given there is no apprenticeship
	When the apprentice is requested
	Then the result should be NotFound

Scenario: There is a server error
	Given there is a server error
	When the apprentice is requested
	Then the result should be the error

