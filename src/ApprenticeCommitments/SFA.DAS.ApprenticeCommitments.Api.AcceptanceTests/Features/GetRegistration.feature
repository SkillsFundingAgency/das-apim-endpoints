Feature: GetRegistration
	When an Apprentice requests their registration details
	As an outer API
	I want to retrieve the details from the inner api

Scenario: Registration does NOT exist
	Given no registration exists
	When retrieving registration details
	Then not found is returned

Scenario: Registration does exist
	Given a registration exists
	When retrieving registration details
	Then Ok is returned
	And the response contains the expected details
