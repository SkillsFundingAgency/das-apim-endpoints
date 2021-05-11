@innerApi
@commitmentsV2InnerApi
@outerApi

Feature: GettingEligibleApprentices
	When the caller requests a list of eligible apprentices for an AccountLegalEntity
	As an Outer Api
	I want to return all the eligible apprentices

Scenario: Request for eligible apprentices but there are no approved apprentices in commitments for this company
	Given the caller wants to search for eligible apprentices by Account Id and AccountLegalEntity Id
	And this search request finds no approved apprenticeships
	When the Outer Api receives the request to list all eligible apprentices
	Then the result should return Ok, but with no apprentices

Scenario: Request for eligible apprentices returns several approved apprentices and two are eligible
	Given the caller wants to search for eligible apprentices by Account Id and AccountLegalEntity Id
	And this search request finds several approved apprenticeships
	And two of these are eligible
	When the Outer Api receives the request to list all eligible apprentices
	Then the result should return Ok and have two apprentices

Scenario: Request for eligible apprentices filters out stopped commitments
	Given the caller wants to search for eligible apprentices by Account Id and AccountLegalEntity Id
	And this search request finds several approved apprenticeships which are stopped
	And two of these are eligible
	When the Outer Api receives the request to list all eligible apprentices
	Then the result should return Ok, but with no apprentices