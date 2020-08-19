@innerApi
@outerApi

Feature: SendBankDetailsRequiredEmail
	In order to complete an application
	As an employer
	I want to be prompted to enter my bank details

Scenario: Send bank details required email
	Given an employer is applying for the New Apprenticeship Incentive
	When employer has indicated that they are unable to provide bank details at present
	Then the employer is sent an email reminding them to supply their bank details