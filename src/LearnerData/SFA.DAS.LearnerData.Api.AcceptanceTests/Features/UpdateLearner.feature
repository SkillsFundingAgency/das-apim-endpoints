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
	When the learner is updated with new earnings profile version
	Then a on-programme update request is sent to the earnings domain
	And a release-earnings update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: MathsAndEnglish updated
	Given there is a learner
	And the EnglishAndMaths passed is different to the value in the learners domain
	When the learner is updated
	Then a english-and-maths update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: OnProgramme LearningSupport updated
	Given there is a learner
	And the OnprogrammeLearningSupport passed is different to the value in the learners domain
	When the learner is updated
	Then a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: EnglishAndMaths LearningSupport updated
	Given there is a learner
	And the EnglishAndMathsLearningSupport passed is different to the value in the learners domain
	When the learner is updated
	Then a english-and-maths update request is sent to the earnings domain
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
	And the EnglishAndMathsWithdrawal passed is different to the value in the learners domain
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

Scenario: Already-approved subsequent apprenticeship is not resent to Approvals (via LearnerData)
	Given there is a learner
	And the learner submits an OnProgramme item for a subsequent apprenticeship
	And that apprenticeship is already approved
	When the learner is updated
	Then no LearnerDataEvent is published

Scenario: New subsequent apprenticeship is sent to Approvals (via LearnerData)
	Given there is a learner
	And the learner submits an OnProgramme item for a subsequent apprenticeship
	And that apprenticeship is not yet approved
	When the learner is updated
	Then a LearnerDataEvent is published

Scenario: LearnerRef is passed through to the learning domain
	Given there is a learner
	And the CompletionDate passed is different to the value in the learners domain
	And the learner has a LearnerRef of "LR-12345"
	When the learner is updated with new earnings profile version
	Then the LearnerRef sent to the learning domain is "LR-12345"
	And the release-earnings request sent to the earnings domain has the learner key and ref "LR-12345"

Scenario: Missing LearnerRef is passed through to the learning domain as empty, not garbage
	Given there is a learner
	And the CompletionDate passed is different to the value in the learners domain
	And the learner has a LearnerRef of ""
	When the learner is updated
	Then the LearnerRef sent to the learning domain is ""

Scenario: Start date change that Approvals auto-approves
	Given there is a learner
	And the StartDate passed is different to the value in the learners domain
	And Learning reports that the change needs further approval
	And Approvals auto-approves the change
	When the learner is updated
	Then Approvals is asked about the start date change
	And further approval needed is cleared in the learning domain
	And a on-programme update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Start date change that Approvals does not auto-approve
	Given there is a learner
	And the StartDate passed is different to the value in the learners domain
	And Learning reports that the change needs further approval
	And Approvals asks for employer approval of the change
	When the learner is updated
	Then Approvals is asked about the start date change
	And further approval needed is not cleared in the learning domain
	And no update request is sent to the earnings domain
	And sld data is stored to the cache

Scenario: Change that does not need further approval does not ask Approvals
	Given there is a learner
	And the Prices passed is different to the value in the learners domain
	And the funding band maximum for that learner is set
	When the learner is updated
	Then Approvals is not asked about the change
	And further approval needed is not cleared in the learning domain
	And a on-programme update request is sent to the earnings domain
