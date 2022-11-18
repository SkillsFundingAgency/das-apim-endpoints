@innerApi
@outerApi
Feature: ReinstatePayments
	When a Reinstate Payments request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to reinstate payments is received
	Given the caller wants to reinstate payments for apprenticeship incentives
	And the Employer Incentives Api receives the request
	When the Outer Api receives the Reinstate Payments request
	Then the response of OK is returned

Scenario Outline: Request to reinstate payments that cannot be found
	Given the caller wants to reinstate payments for apprenticeship incentives
	And the Employer Incentives Api receives the request but returns a payment not found status
	When the Outer Api receives the Reinstate Payments request
	Then the response of Bad Request is returned
	And the response body contains the full message