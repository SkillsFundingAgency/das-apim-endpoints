@innerApi
@outerApi

Feature: SendBankDetailsRepeatReminderEmails
	In order to receive the employer incentive payment
	As an employer
	I want to be reminded to enter my bank details if they have not been supplied

Scenario: Send bank details reminder email
	Given an employer has applied for the employer incentive
	When employer has not supplied their bank details
	Then the employer is sent an email reminding them how to supply their bank details so that they can receive payment