Feature: GetApprenticeHomePage
	As an apprentice
	I need to retrieve my home page summary details

Scenario: The apprentice exists
	Given there is an apprentice
	When the apprentice's homepage is requested
	Then the result should contain the apprentice data, but with no apprenticeship data or my apprenticeship data

Scenario: The apprentice exists and has multiple apprenticeships 
	Given there is an apprentice
	And several apprenticeships
	And my apprenticeship exists
	When the apprentice's homepage is requested
	Then the result should have apprentice and first apprenticeship and MyApprenticeship

Scenario: The apprentice does not exists but has multiple apprenticeships 
	Given there is no apprentice
	And several apprenticeships
	When the apprentice's homepage is requested
	Then the result should be NotFound
