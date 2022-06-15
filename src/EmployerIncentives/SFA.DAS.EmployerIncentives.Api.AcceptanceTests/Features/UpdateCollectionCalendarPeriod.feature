@innerApi
@outerApi

Feature: UpdateCollectionCalendarPeriod
	When a request to update a collection calendar period is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to update a collection calendar period
	Given the caller wants to update a collection calendar period
	And the Employer Incentives Api receives the collection calendar period update request
	When the Outer Api receives the collection calendar period update request
	Then the response code of Ok is returned