Feature: RemoveLearner

These tests validate the functionality of removing a learner from the system.

Scenario: Remove Learner
	Given there is a learner
	When the learner is removed
	Then a remove learning request is sent to the learning domain
	And a delete learner request is sent to the earnings domain