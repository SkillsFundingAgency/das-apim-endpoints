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

Scenario Outline: Price and learning type sent to earnings domain reflect the data from the Courses API
	Given there is a provider
	And the courses api has the following funding bands:
		| From       | To         | Price |
		| 2020-01-01 | 2022-12-31 | 6000  |
		| 2023-01-01 |            | 9000  |
	When a draft short course is created for the provider with start date <StartDate> and learning type <LearningType>
	Then the earnings domain receives a price of <ExpectedPrice> and learning type <LearningType>

	Examples:
		| StartDate  | LearningType             | ExpectedPrice |
		| 2021-06-01 | ApprenticeshipUnit       | 6000          |
		| 2024-03-01 | FoundationApprenticeship | 9000          |
