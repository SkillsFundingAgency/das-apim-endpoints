Feature: UpdateShortCourseLearning

A short summary of the feature

Scenario: Withdrawal
	Given there is a short course learning
	And the WithdrawalDate of short course passed is different to the value in the learning domain
	When the short course learning is updated
	Then a on-programme update request is sent for short courses to the earnings domain

Scenario: Completion
	Given there is a short course learning
	And the CompletionDate of short course passed is different to the value in the learning domain
	When the short course learning is updated
	Then a on-programme update request is sent for short courses to the earnings domain
