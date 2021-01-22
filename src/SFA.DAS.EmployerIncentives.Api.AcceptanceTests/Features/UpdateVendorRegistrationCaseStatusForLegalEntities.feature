@innerApi
@customerEngagementFinanceApi
@outerApi

Feature: UpdateVendorRegistrationCaseStatusForLegalEntities
	In order for Incentive Application to progress
	As an employer
	I want VRF case status to be updated

Scenario: A VRF case status update job is triggered
	When Refresh Vendor Registration Form Status is invoked
	Then latest VRF cases are retrieved from Finance API
	And VRF case details are updated for legal entities

Scenario: A VRF case status update job is triggered with a mix of case types
	When Refresh Vendor Registration Form Status is invoked and New and other case types exist in VRF
	Then latest VRF cases are retrieved from Finance API
	And VRF case details are updated only for legal entities with a case type of New