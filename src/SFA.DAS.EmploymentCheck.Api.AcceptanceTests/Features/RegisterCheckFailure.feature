@innerApi
@outerApi

Feature: RegisterCheckFailure
	In order to confirm the employment for given apprentice in a given time period
	As an employer incentives service
	I want to be able to register a check for apprentice's employment status

Scenario: Employment Check fails to register
	Given an employer has applied for Apprenticeship Incentive for an apprentice
	When the Employer Incentives service are checking employment status of the apprentice
	And the employment check request has failed validation
	Then an error response is returned by the Employment Check system