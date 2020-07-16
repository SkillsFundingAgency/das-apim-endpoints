@innerApi
@outerApi
Feature: LegalEntityRequests
	When a LegalEntity request is made 
	As an Outer Api
	I want to simply forward that request to the Employer Incentives Api

Scenario: Request to add a LegalEntity to an Account 
	Given the caller wants to add a LegalEntity to an Account
	And the Employer Incentives Api receives the add request
	When the Outer Api receives the add request
	Then the response from the Employer Incentives Inner Api is returned

Scenario: Request to add a LegalEntity to an Account but an internal error occurs
	Given the caller wants to add a LegalEntity to an Account
	And the Employer Incentives Api will error when receiving the add request
	When the Outer Api receives the add request
	Then the response from the Employer Incentives Inner Api has no content

Scenario: Request to remove a LegalEntity from an Account 
	Given the caller wants to remove a LegalEntity from an Account
	And the Employer Incentives Api receives the remove request
	When the Outer Api receives the remove request
	Then the response from the Employer Incentives Inner Api has no content

Scenario: Request to get all LegalEntities for an Account 
	Given the caller wants to get all LegalEntities for an Account
	And the Employer Incentives Api receives the query request
	When the Outer Api receives the query request
	Then the response from the Employer Incentives Inner Api is returned
