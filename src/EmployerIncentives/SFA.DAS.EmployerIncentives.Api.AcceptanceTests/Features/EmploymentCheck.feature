@innerApi
@outerApi

Feature: EmploymentCheck
	When a request to update an employment check is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to update an employment check
	Given the caller wants to update an employment check
	And the Employer Incentives Api receives the employment check update request
	When the Outer Api receives the employment check update request
	Then the response code of Ok is returned

Scenario Outline: Request to register an employment check
	Given the caller wants to register an employment check
	And the Employer Incentives Api receives the register employment check request
	When the Outer Api receives the register employment check request
	Then the response code of Ok is returned