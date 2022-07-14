@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: ApprenticeshipDetails
	In order to process my apprenticeships application
	As an employer
	I want to be confirm the employment start dates of the apprentices that I have applied for

Scenario: Incentive Application Employment Details provided
	Given an employer is applying for the New Apprenticeship Incentive
	And employer has selected the apprenticeships for the application
	When the employer confirms the employment start dates for their apprentices
	Then the application is updated with the apprentices employment details