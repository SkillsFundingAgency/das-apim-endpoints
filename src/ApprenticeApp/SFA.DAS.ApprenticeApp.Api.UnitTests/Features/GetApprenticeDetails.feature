Feature: GetApprenticeDetails
	As an apprentice
	I need to retrieve my details, 
	It should collect the apprenticeships from CMAD and MyApprenitceship from Apprentice Accounts and lookup the course title
	either using a Framework course of a Standard course

Scenario: The apprentice exists
	Given there is an apprentice
	When the apprentice's details are requested
	Then the result should contain the apprentice data, but with no apprenticeship data or my apprenticeship data

Scenario: The apprentice exists and has multiple apprenticeships and a Standard MyApprenticeship
	Given there is an apprentice
	And my apprenticeship exists
	And my apprenticeship has a standard course
	When the apprentice's details are requested
	Then the result should have apprentice and first apprenticeship
	And a Standard MyApprenticeship course

Scenario: The apprentice exists and has multiple apprenticeships and a Framework MyApprenticeship
	Given there is an apprentice
	And my apprenticeship exists
	And my apprenticeship has a framework course
	When the apprentice's details are requested
	Then the result should have apprentice and first apprenticeship
	And a Framework MyApprenticeship course

Scenario: The apprentice does not exists but has multiple apprenticeships 
	Given there is no apprentice
	When the apprentice's details are requested
	Then the result should be NotFound
