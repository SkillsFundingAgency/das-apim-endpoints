Feature: PatchApprentice
	As the portal APPLICATION
	I need to be able to pass a PATCH request through to the inner API

Scenario: The apprentice exists
	When an apprentice patch request to update the email address is received
	Then the patch request should be passed to the inner API
	And it contains all the information
