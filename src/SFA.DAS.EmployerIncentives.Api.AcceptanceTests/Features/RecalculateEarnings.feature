@innerApi
@outerApi

Feature: RecalculateEarnings
	When a request to recalculate earnings is made
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api


Scenario: Request to recalculate earnings for apprenticeship incentices
	Given the caller wants to recalculate earnings
	And the Employer Incentives Api receives the recalculate earnings request
	When the Outer Api receives the recalculate earnings request
	Then the respsonde code of NoContent is returned
