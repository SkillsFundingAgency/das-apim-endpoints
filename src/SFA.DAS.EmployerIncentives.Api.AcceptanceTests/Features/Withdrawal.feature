@innerApi
@outerApi
Feature: Withdrawal
	When a Withdrawal request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to add an Withdrawal request
	Given the caller wants to Withdraw an apprenticeship application
	And the Employer Incentives Api receives the Withdrawal request
	When the Outer Api receives the Withdrawal request
	Then the response of Accepted is returned

Scenario Outline: Request to add an Reinstate request
	Given the caller wants to Reinstate an apprenticeship application
	And the Employer Incentives Api receives the Reinstate request
	When the Outer Api receives the Reinstate request
	Then the response of Accepted is returned