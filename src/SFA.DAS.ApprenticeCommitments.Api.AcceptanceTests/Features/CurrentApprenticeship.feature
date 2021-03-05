@innerApi
Feature: CurrentApprenticeship
	As an apprentice
	I need to have a start screen showing all the steps of my commitment statement
	So that I know what steps are involved in signing my commitment statement
	And I can be directed to sign every aspect of my statement

Scenario: Current apprenticeship overview
	Given an apprentice has registered
	And there is a current apprenticeship
	When the current apprenticeship overview is requested
	Then the result should be OK
	Then and the current apprenticeship is returned

Scenario: No current apprenticeship
	Given there is no current apprenticeship
	When the current apprenticeship overview is requested
	Then the result should be NotFound

