@innerApi
@outerApi
Feature: PausePayments
	When a Pause/Resume Payments request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to pause payments is received
	Given the caller wants to pause payments for an apprenticeship incentive
	And the Employer Incentives Api receives request
	When the Outer Api receives the Pause Payments request
	Then the response of OK is returned

Scenario Outline: Request to pause payments that do not exist
	Given the caller wants to pause payments for an apprenticeship incentive
	And the Employer Incentives Api receives request but cannot find the incentive
	When the Outer Api receives the Pause Payments request
	Then the response of Not Found is returned
	And the response body contains the full message

Scenario Outline: Request to pause payments that are already paused
	Given the caller wants to pause payments for an apprenticeship incentive
	And the Employer Incentives Api receives request but returns an already paused status
	When the Outer Api receives the Pause Payments request
	Then the response of Bad Request is returned
	And the response body contains the full message