﻿Feature: GetApprenticeship
	As an apprentice
	I need to retrieve an individual apprenticeship
	So I can review the details and confirm it

Scenario: The apprenticeship exists
	Given there is an apprenticeship
	When the apprenticeship is requested
	Then the result should be OK
	And the result should contain the anticipated values

Scenario: The apprenticeship does not exist
	Given there is no apprenticeship
	When the apprenticeship is requested
	Then the result should be NotFound