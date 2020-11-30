@innerApi
@outerApi

Feature: ActivateCollectionCalendarPeriod
	When a request to activate a collection calendar period is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to activate a collection calendar period
	Given the caller wants to activate a collection calendar period
	And the Employer Incentives Api receives the collection calendar period activation request
	When the Outer Api receives the collection calendar period activation request
	Then the response code of Ok is returned