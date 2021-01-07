@innerApi
@customerEngagementFinanceApi
@outerApi

Feature: UpdateVendorRegistrationCaseStatusForLegalEntities
	In order for Incentive Application to progress
	As an employer
	I want VRF case status to be updated

Scenario: A VRF case status update job is triggered
	When Refresh Vendor Registration Form Status is invoked
	Then the latest update date is retrieved from Employer Incentives API 
	And VRF case updates are retrieved from Finance API
	And VRF case details are updated for legal entities

