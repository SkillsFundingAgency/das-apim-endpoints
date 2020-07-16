@innerApi
@commitmentsV2InnerApi
@outerApi
Feature: HealthChecks
	When a healthcheck is requested
	As an Outer Api
	I want return the status

Scenario: Ping HealthChecks is called
	Given the Employer Incentives Inner Api is ready and healthy
	And the Commitments Inner Api is ready and healthy
	When I ping the Outer Api
	Then the result should show healthy
