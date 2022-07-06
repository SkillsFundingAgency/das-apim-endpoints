@innerApi
@outerApi
Feature: RevertPayments
	When a Revert Payments request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to revert payments is received
	Given the caller wants to revert payments for an apprenticeship incentive
	And the Employer Incentives Api receives the request
	When the Outer Api receives the Revert Payments request
	Then the response of OK is returned

Scenario Outline: Request to revert payments that cannot be found
	Given the caller wants to revert payments for an apprenticeship incentive
	And the Employer Incentives Api receives request but returns a payment not found status
	When the Outer Api receives the Revert Payments request
	Then the response of Bad Request is returned
	And the response body contains the full message