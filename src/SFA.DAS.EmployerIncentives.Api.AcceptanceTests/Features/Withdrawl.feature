@innerApi
@outerApi
Feature: Withdrawl
	When a Withdrawl request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to add an Withdrawl request
	Given the caller wants to Withdraw an apprenticeship application
	And the Employer Incentives Api receives the WithDrawl request
	When the Outer Api receives the WithDrawl request
	Then the response of Accepted is returned