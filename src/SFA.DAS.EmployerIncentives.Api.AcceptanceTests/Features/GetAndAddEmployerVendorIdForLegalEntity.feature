@innerApi
@customerEngagementFinanceApi
@outerApi
Feature: GetAndAddEmployerVendorIdForLegalEntity
	In order for Legal Entity to be paid by Business Central
	As an employer
	I want the Employer Vendor Id to be added to the Legal Entity

Scenario: A VRF case status is updated and Employer Vendor Id is returned
	When Get and Add Employer Vendor Id is invoked
	Then employer vendor Id is returned from the Finance API
	And the legal entity is sent an update of the employer vendor Id

Scenario: A VRF case status is updated and Employer Vendor Id is not returned
	When Get and Add Employer Vendor Id is invoked
	Then employer vendor Id is not returned from the Finance API
	And the legal entity is not sent an update of the employer vendor Id