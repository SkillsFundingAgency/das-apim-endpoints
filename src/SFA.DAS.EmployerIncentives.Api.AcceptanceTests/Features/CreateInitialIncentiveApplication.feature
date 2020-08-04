@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: CreateInitialIncentiveApplication
	When the employer has selected apprentices and wants to save the initial incentive application
	As an Outer Api
	I want to get the apprenticeship details and save the initial incentive application

Scenario: Employer wants to save the initial incentive application
	Given the employer has selected a few apprentices
	And the apprenticeships are all found and valid
	When the initial incentive application is saved
	Then the response should be Created