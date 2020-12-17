@innerApi
@outerApi
Feature: PausePayments
	When a Pause/Resume Payments request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to pause/resume payments is received
	Given the caller wants to pause/resume payments for an apprenticeship incentive
	And the Employer Incentives Api receives request
	When the Outer Api receives the Pause Payments request
	Then the response of OK is returned