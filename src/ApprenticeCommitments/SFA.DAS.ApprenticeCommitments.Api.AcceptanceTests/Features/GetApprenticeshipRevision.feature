Feature: GetApprenticeshipRevision
	As an apprentice
	I need to retrieve an individual apprenticeship revision
	So I can review the details on that revision

Scenario: The apprenticeship exists
	Given there is an apprenticeship revision
	When an explicit apprenticeship revision is requested
	Then the result should be OK
	And the result should have the correct Content-Type
	And the result should contain the anticipated values

Scenario: The apprenticeship does not exist
	Given there is no apprenticeship revision
	When an explicit apprenticeship revision is requested
	Then the result should be NotFound