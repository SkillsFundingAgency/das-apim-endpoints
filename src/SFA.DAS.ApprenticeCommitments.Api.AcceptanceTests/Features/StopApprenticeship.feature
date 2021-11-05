@loginApi
Feature: StopApprenticeship

Scenario: Stop an apprenticeship
	When an apprenticeship stop is posted
	Then the response should be OK
	And the inner API has received the posted values
