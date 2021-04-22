@innerApi
@loginApi
@commitmentsV2InnerApi

Feature: SendInvitationReminders
	Periodically the system will check to see if any invitation reminders need to be sent out
	As an outer API
	I want to get the registrations which need reminders, and send the reminders and say it's been sent
	
Scenario: No reminders are found, nothing should be sent
	Given there are no reminders
	When the scheduled job starts process for sending reminders
	Then return an ok response
	And no invitations are sent

Scenario: One reminders is found, the invitations is sent and marked as being sent
	Given there is one reminder
	And the course names are found
	When the scheduled job starts process for sending reminders
	Then return an ok response
	And the invitation is sent with expected values
	And the registration is marked as sent

Scenario: Reminders are found, the invitations should be sent and marked as being sent
	Given there are reminders
	And the course names are found
	When the scheduled job starts process for sending reminders
	Then return an ok response
	And all invitations are sent
