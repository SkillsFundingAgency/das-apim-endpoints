@employmentCheckInnerApi
@outerApi

Feature: RegisterEmploymentCheck

Scenario Outline: Request to register an employment check
	Given the caller wants to register an employment check
	And the Employment Check Api receives the register employment check request
	When the Outer Api receives the register employment check request
	Then the response code of Ok is returned