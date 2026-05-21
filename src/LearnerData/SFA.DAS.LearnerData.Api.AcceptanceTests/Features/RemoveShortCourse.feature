Feature: RemoveShortCourse

These tests validate the functionality of removing a short course learner

Scenario: Short course is removed
	Given there is a short course learning
	When the short course is removed
	Then a remove short course request is sent to the learning domain
	And a remove short course request is sent to the earnings domain
	And a short course earnings updated event is published for payments

