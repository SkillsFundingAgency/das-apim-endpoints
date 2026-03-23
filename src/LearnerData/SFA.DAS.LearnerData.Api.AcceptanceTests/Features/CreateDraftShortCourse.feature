Feature: CreateDraftShortCourse

Scenario: Learning processes the request and earnings is updated
	Given there is a provider
	When a draft short course is created for the provider
	Then a short course creation request is sent to the earnings domain

Scenario: Learning ignores the request due to an unhandled scenario
	Given there is a provider
	And the learning domain will return no content for the short course creation
	When a draft short course is created for the provider
	Then the earnings domain is not called
