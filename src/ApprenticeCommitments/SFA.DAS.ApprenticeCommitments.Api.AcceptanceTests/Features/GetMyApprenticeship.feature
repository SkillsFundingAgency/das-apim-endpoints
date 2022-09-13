Feature: GetMyApprenticeship
	As an apprentice
	I need to retrieve an individual apprenticeship
	So I can review the details

Scenario: The apprenticeship exists
	Given there is a confirmed apprenticeship
	When the latest confirmed apprenticeship is requested
	Then the result should be OK
	And the result should have the correct Content-Type
	And the result should contain the anticipated values

Scenario: The apprenticeship does not exist
	Given there is no confirmed apprenticeship
	When the latest confirmed apprenticeship is requested
	Then the result should be NotFound