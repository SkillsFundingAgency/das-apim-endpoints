@loginApi
Feature: CreateApprenticeship

Create an apprenticeship by matching an apprentice to an approval

Scenario: Success
	Given an account
	When the account is matched to an approval
	Then the match succeeds
	And the account details are passed to the API
