@innerApi
Feature: ConfirmApprenticeshipFeature

Scenario: Confirm the training provider
	Given a Training Provider confirmation
	And the inner API will accept the confirmation
	When we confirm the training provider
	Then return an ok response
