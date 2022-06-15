Feature: GetApprovalsRegistration
	When we request registration details for an approvals identity
	As an outer API
	I want to retrieve the details from the inner api and return this

Scenario: Approvals Registration does NOT exist
	Given no approvals registration exists
	When retrieving approval registration details
	Then not found is returned

Scenario: Approvals Registration does exist
	Given an approvals registration exists
	When retrieving approval registration details
	Then Ok is returned
	And the response returns the inner api response
