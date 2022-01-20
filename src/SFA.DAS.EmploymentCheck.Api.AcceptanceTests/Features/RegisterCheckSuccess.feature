@innerApi
@outerApi

Feature: RegisterCheckSuccess
	In order to register apprentice's employment check
	As an employer incentives service
	I want to be confirm the employment for given apprentice in a given time period

Scenario: Employment Check is registered
	Given an employer has applied for Apprenticeship Incentive for an apprentice
	When the Employer Incentives service are validating employment of the apprentice
	Then a new Employment Check request is registered in Employment Check system