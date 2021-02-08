@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: GetApplicationsForAccount
	When the caller requests a list of apprentice applications for an account
	As an Outer Api
	I want to return all the eligible apprentice applications


Scenario: Request for apprentice applications for this account but there are no applications yet
	Given the caller wants to search for apprentice applications by Account Id and Account Legal Entity Id
	And this search request finds no applications
	When the Outer Api receives the request to list all applications
	Then the result should return Ok, but with no applications

Scenario: Request for apprentice applications for this account and there is one application submitted for a single apprentice
	Given the caller wants to search for apprentice applications by Account Id and Account Legal Entity Id
	And this search request finds one submitted application
	When the Outer Api receives the request to list all applications
	Then the result should return Ok, with one submitted application

Scenario: Request for apprentice applications for this account and there is one application submitted for multiple apprentices
	Given the caller wants to search for apprentice applications by Account Id and Account Legal Entity Id
	And this search request finds one application for multiple apprentices
	When the Outer Api receives the request to list all applications
	Then the result should return Ok, with one submitted application for multiple apprentices

Scenario: Request for apprentice applications for this account and there are multiple applications submitted
	Given the caller wants to search for apprentice applications by Account Id and Account Legal Entity Id
	And this search request finds multiple submitted applications
	When the Outer Api receives the request to list all applications
	Then the result should return Ok, with multiple submitted applications