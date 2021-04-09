@innerApi
Feature: ConfirmApprenticeshipFeature

Scenario: Confirm the training provider
	Given a Training Provider confirmation
	And the inner API will accept the confirmation
	When we confirm the training provider
	Then return an ok response

Scenario: Confirm the employer
	Given an Employer confirmation is requested
	And the inner API will accept the confirmation
	When we confirm the employer
	Then return an ok response

Scenario: Confirm the roles and responsibilities
	Given a Roles and Responsibilities confirmation is requested
	And the inner API will accept the confirmation
	When we confirm the roles and responsibilities
	Then return an ok response