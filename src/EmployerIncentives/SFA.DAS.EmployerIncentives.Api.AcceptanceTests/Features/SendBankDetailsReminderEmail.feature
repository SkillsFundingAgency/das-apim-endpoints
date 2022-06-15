@innerApi
@outerApi

Feature: SendBankDetailsReminderEmail
	In order to complete an application
	As an employer
	I want to be prompted to complete the application by entering my bank details

Scenario: Send bank details reminder email
	Given an employer is applying for the New Apprenticeship Incentive
	When employer has indicated that they are able to enter their bank details
	Then the employer is sent an email reminding them how to supply their bank details in case they are unable to complete the application