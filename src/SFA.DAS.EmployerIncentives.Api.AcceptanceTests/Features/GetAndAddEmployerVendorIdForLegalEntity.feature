@innerApi
@customerEngagementFinanceApi
@outerApi

Feature: GetAndAddEmployerVendorIdForLegalEntity
	In order for Legal Entity to be paid by Business Central
	As an employer
	I want the Employer Vendor Id to be added to the Legal Entity

Scenario: A VRF case status of 'Request Completed' is received and this invokes a trigger to get the Employer Vendor Id
	When Get and Add Employer Vendor Id is invoked
	Then employer vendor Id is retrieved from the Finance API
	And the legal entity is sent an update of the employer vendor Id