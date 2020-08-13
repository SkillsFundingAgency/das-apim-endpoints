@innerApi
@outerApi
Feature: JobRequests
	When a Job request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to add a Job request
	Given the caller wants to start a RefreshLegalEntities EmployerIncentives Job
	And the Employer Incentives Api receives the Job request
	When the Outer Api receives the Job request
	Then the response of NoContent is returned
