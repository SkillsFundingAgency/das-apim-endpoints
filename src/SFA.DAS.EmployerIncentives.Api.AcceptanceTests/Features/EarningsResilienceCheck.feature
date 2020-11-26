@innerApi
@outerApi

Feature: EarningsResilienceCheck
	When an earnings resilience check request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to perform an earnings resilience check
	Given the caller wants to perform an earnings resilience check
	And the Employer Incentives Api receives the earnings resilience check request
	When the Outer Api receives the earnings resilience check request
	Then the response code of Ok is returned