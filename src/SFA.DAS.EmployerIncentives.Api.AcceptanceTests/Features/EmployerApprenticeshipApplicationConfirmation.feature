@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: ApplicationConfirmation
	As an employer applying for the new apprentice grant
	If I have selected and confirmed apprenticeship selection
	I want to be able to view and accept the declaration

Scenario: An employer is viewing and accepting declaration
	Given an employer applying for a grant is asked to agree a declaration
	When the employer understands and confirms the declaration
	Then then the employer application declaration is accepted