Feature: ApprenticeViewedRevision
	As an apprentice
	I need to say I just viewed a revision
	So I can know that I have seen it

Scenario: The revision has been viewed 
	Given the inner api is ready
	When the last viewed is set
	Then the result should be OK
	And the last viewed date and time should have been passed to inner api