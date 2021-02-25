@innerApi
Feature: ChangeEmailAddress
	As an apprentice
	I want to be able to change the email address associated with my digital account
	So that I can still access my commitment & receive updates from the service

Scenario: A valid verify identity and registration request is received
	Given the requested change is valid
	And the inner API will accept the change
	When we change the apprentice's email address
	Then return an ok response
