Feature: GetApprenticeHomePage
	As an apprentice
	I need to retrieve my home page summary details

Scenario: The apprentice exists
	Given there is an apprentice
	When the apprentice is requested
	Then the result should contain the apprentice data

Scenario: The apprentice exists but no apprenticeships exist
	Given there is no apprenticeship
	When the apprentice is requested
	Then the result should be NotFound

#Scenario: There is a server error getting 
#	Given there is a server error
#	When the apprentice is requested
#	Then the result should be the error
