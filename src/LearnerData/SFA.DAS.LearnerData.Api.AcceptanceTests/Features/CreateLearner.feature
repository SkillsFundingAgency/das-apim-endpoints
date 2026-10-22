Feature: CreateLearner

These tests validate creating apprenticeship learners. A POST for a learner whose approved apprenticeship was removed
reinstates it, and the reinstated episode is still approved, so a change it makes (e.g. to the start date) can need further approval.

Scenario: Reinstatement with a start date change that Approvals auto-approves
	Given there is an approved apprenticeship that was removed
	And the reinstating submission changes the start date
	And Learning reports that the change needs further approval
	And Approvals auto-approves the change
	When the learner is reinstated
	Then Approvals is asked about the start date change
	And further approval needed is cleared in the learning domain
	And an ApprovedLearningUpdatedEvent is published for the start date change
	And a on-programme update request is sent to the earnings domain
	And the on-programme update request sent to the earnings domain does not require further approval

Scenario: Reinstatement with a start date change that Approvals does not auto-approve
	Given there is an approved apprenticeship that was removed
	And the reinstating submission changes the start date
	And Learning reports that the change needs further approval
	And Approvals asks for employer approval of the change
	When the learner is reinstated
	Then Approvals is asked about the start date change
	And further approval needed is not cleared in the learning domain
	And no ApprovedLearningUpdatedEvent is published
	And a on-programme update request is sent to the earnings domain
	And the on-programme update request sent to the earnings domain requires further approval

Scenario: Reinstatement without a change that needs further approval does not ask Approvals
	Given there is an approved apprenticeship that was removed
	When the learner is reinstated
	Then Approvals is not asked about the change
	And further approval needed is not cleared in the learning domain
	And no ApprovedLearningUpdatedEvent is published
	And a on-programme update request is sent to the earnings domain
	And the on-programme update request sent to the earnings domain does not require further approval
