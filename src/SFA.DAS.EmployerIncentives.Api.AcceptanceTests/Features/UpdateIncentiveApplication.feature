@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: UpdateIncentiveApplication
	In order to modify a draft apprenticeships application
	As an employer
	I want to be able to modify my selection of apprenticeships

Scenario: Incentive Application is updated
	Given an employer is applying for the New Apprenticeship Incentive
	When employer has changed selected apprenticeships for the application
	Then the application is updated with new selection of apprenticeships