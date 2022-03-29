@innerApi
@outerApi

Feature: RegisterCheckSuccess
	In order to confirm the employment for given apprentice in a given time period
	As an employer incentives service
	I want to be able to register a check for apprentice's employment status

Scenario: Employment Check is registered
	Given an employer has applied for Apprenticeship Incentive for an apprentice
	When the Employer Incentives service are checking employment status of the apprentice
	And the employment check request has passed validation
	Then a new Employment Check request is registered in Employment Check system