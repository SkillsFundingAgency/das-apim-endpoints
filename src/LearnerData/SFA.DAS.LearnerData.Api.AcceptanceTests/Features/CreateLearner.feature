Feature: CreateLearner

This feature covers creation of apprenticeship learners.
todo: Coverage is largely missing for CreateLearner, the scenario below has been added for the FLP-2138 bug fix, tech debt item to add more coverage for CreateLearner to follow.

Scenario: English and maths period in learning after a break starts on the learner's return date
	Given a provider is creating an apprenticeship learner
	And the learner has the following English and maths items for the same course:
		| StartDate  | EndDate    | PauseDate  | CompletionDate |
		| 2026-08-01 | 2026-11-30 | 2026-09-30 |                |
		| 2027-02-01 | 2027-05-31 |            | 2027-03-15     |
	When the apprenticeship learner is created
	Then the earnings domain receives the following English and maths periods in learning:
		| StartDate  | EndDate    | OriginalExpectedEndDate |
		| 2026-08-01 | 2026-09-30 | 2026-11-30              |
		| 2027-02-01 | 2027-03-15 | 2027-05-31              |
