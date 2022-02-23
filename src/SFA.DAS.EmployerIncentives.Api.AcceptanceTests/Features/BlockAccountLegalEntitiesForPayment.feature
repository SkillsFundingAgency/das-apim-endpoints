@innerApi
@outerApi

Feature: BlockAccountLegalEntitiesForPayment
	When a request to block account legal entities for payments is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario Outline: Request to block account legal entities for payment
	Given the caller wants to block account legal entities for payment
	And the Employer Incentives Api receives the block payments request
	When the Outer Api receives the block payments request
	Then the response code of NoContent is returned