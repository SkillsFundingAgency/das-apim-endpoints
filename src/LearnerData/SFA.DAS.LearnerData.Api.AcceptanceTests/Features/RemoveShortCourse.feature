Feature: RemoveShortCourse

These tests validate the functionality of removing a short course learner

Scenario: Short course is removed
	Given there is a short course learning
	When the short course is removed
	Then a remove short course request is sent to the learning domain
	And a remove short course request is sent to the earnings domain
	And a short course earnings updated event is published for payments

Scenario: Learning returns no content - earnings domain is not called
	Given there is a short course learning
	And the learning domain will return no content for the short course removal
	When the short course is removed
	Then the earnings domain is not called for short course removal
