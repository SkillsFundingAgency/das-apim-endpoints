Feature: UpdateLearner

These tests validate the functionality of updating learner details in the system.

Scenario: No changes made
	Given there is a learner
	And the details passed in are the same as the existing learner details
	When the learner is updated
	Then no changes are made to the learner

Scenario: Completed date updated
	Given there is a learner
	And the CompletionDate passed is different to the value in the learners domain
	When the learner is updated
	Then a CompletionDate update request is sent to the earnings domain

Scenario: MathsAndEnglish updated
	Given there is a learner
	And the MathsAndEnglish passed is different to the value in the learners domain
	When the learner is updated
	Then a MathsAndEnglish update request is sent to the earnings domain

Scenario: LearningSupport updated
	Given there is a learner
	And the LearningSupport passed is different to the value in the learners domain
	When the learner is updated
	Then a LearningSupport update request is sent to the earnings domain

Scenario: Prices updated
	Given there is a learner
	And the funding band maximum for that learner is set
	And the Prices passed is different to the value in the learners domain
	When the learner is updated
	Then a Prices update request is sent to the earnings domain

Scenario: Withdrawal
	Given there is a learner
	And the Withdrawal passed is different to the value in the learners domain
	When the learner is updated
	Then a Withdrawal update request is sent to the earnings domain

Scenario: Start Break in Learning
	Given there is a learner
	And the BreakInLearningStarted passed is different to the value in the learners domain
	When the learner is updated
	Then a BreakInLearningStarted update request is sent to the earnings domain

Scenario: Remove Break in Learning
	Given there is a learner
	And the BreakInLearningRemoved passed is different to the value in the learners domain
	When the learner is updated
	Then a BreakInLearningRemoved update request is sent to the earnings domain

Scenario: Update Breaks in Learning
	Given there is a learner
	And the BreaksInLearningUpdated passed is different to the value in the learners domain
	When the learner is updated
	Then a BreaksInLearningUpdated update request is sent to the earnings domain
