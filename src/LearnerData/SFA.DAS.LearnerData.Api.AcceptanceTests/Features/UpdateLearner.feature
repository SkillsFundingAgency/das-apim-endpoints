Feature: UpdateLearner

These tests validate the functionality of updating learner details in the system.

Scenario: No changes made
	Given there is a learner
	And the details passed in are the same as the existing learner details
	When the learner is updated
	Then no changes are made to the learner
	And sld data is stored to the cache

Scenario: Completed date updated
	Given there is a learner
	And the CompletionDate passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: MathsAndEnglish updated
	Given there is a learner
	And the MathsAndEnglish passed is different to the value in the learners domain
	When the learner is updated
	Then a english-and-maths update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: LearningSupport updated
	Given there is a learner
	And the LearningSupport passed is different to the value in the learners domain
	When the learner is updated
	Then a learning-support update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Prices updated
	Given there is a learner
	And the funding band maximum for that learner is set
	And the Prices passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Withdrawal
	Given there is a learner
	And the Withdrawal passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Start Break in Learning
	Given there is a learner
	And the BreakInLearningStarted passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain

Scenario: Remove Break in Learning
	Given there is a learner
	And the BreakInLearningRemoved passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Update Breaks in Learning
	Given there is a learner
	And the BreaksInLearningUpdated passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache
	
Scenario: English and Maths Withdrawal
	Given there is a learner
	And the MathsAndEnglishWithdrawal passed is different to the value in the learners domain
	When the learner is updated
	Then a english-and-maths update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Date of Birth updated
	Given there is a learner
	And the DateOfBirthChanged passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Care updated
	Given there is a learner
	And the Care passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache