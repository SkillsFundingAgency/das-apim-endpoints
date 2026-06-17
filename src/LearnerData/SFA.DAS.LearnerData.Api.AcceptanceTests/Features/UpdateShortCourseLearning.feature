Feature: UpdateShortCourseLearning

A short summary of the feature

Scenario Outline: Withdrawal
	Given there is an <approval> short course learning
	And SLD inform us that the WithdrawalDate has changed
	When the short course learning is updated
	Then a on-programme update request is sent for short courses to the earnings domain
	And a short course earnings updated event is published for payments

	Examples:
		| approval   |
		| approved   |
		| unapproved |

Scenario Outline: Completion
	Given there is an <approval> short course learning
	And SLD inform us that the CompletionDate has changed
	When the short course learning is updated
	Then a on-programme update request is sent for short courses to the earnings domain
	And a short course earnings updated event is published for payments

	Examples:
		| approval   |
		| approved   |
		| unapproved |

Scenario: StartDate change on unapproved episode
	Given there is an unapproved short course learning
	And SLD inform us that the StartDate has changed
	When the short course learning is updated
	Then a on-programme update request is sent for short courses to the earnings domain