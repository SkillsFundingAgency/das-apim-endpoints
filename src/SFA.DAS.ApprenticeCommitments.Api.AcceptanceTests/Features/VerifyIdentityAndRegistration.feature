Feature: VerifyIdentityAndRegistration
	When a Apprentice first logs in, we want to verify their identity and registration
	As an outer API
	I want to receive the identity details, validate their identity and verify the registration

Scenario: A valid verify identity and registration request is received
	Given the request is valid
	And the inner api will verify registration successfully
	When we confirm the identity and verify registration
	Then return an ok response
	And call to inner api mapped fields correctly

Scenario: A valid verify identity and registration request is received a second time
	Given the request is valid
	And the inner api will not verify registration successfully
	When we confirm the identity and verify registration
	Then we receive a bad response from the inner api
	And the inner api error response is returned