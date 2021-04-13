@innerApi
Feature: RegistrationFirstSeen

Scenario: Record when the user first Seens the registration process
	When the outer api recieves a registration first seen request
	Then the outer api returns an accepted response