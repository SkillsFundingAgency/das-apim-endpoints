@loginApi
@innerApi
@commitmentsV2InnerApi
Feature: AddApprenticeship
	When an Apprenticeship is approved and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Course Name             |
	| 1  | Alexa      | Armstrong | Artificial Intelligence |
	| 2  | Zachary    | Zimmerman | Zoology                 |

Scenario: New apprenticeship is recieved and is valid 
	When the following apprenticeship is posted
	| ApprenticeshipId | Email         | Organisation |
	| 1                | Test@Test.com | Apple        |
	Then the inner API was called successfully
	And the invitation was sent successfully

Scenario: New apprenticeship is recieved and is Not valid 
	When the following apprenticeship is posted
	| ApprenticeshipId | Email         | Organisation |
	| 1                | invalidemail | Apple        |
	Then the inner API should return these errors
	| field | error     |
	| email | Not valid |
	And the invitation was not sent