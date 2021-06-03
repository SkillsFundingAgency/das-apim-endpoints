@loginApi
Feature: AddApprenticeship
	When an Apprenticeship is approved and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Course Name             | Course Code |
	| 1  | Alexa      | Armstrong | Artificial Intelligence | 9001        |
	| 2  | Zachary    | Zimmerman | Zoology                 | 9002        |

	Given the following training providers exist
	| Ukprn | Legal Name   | Trading Name    |
	| 1001  | My Real Name | My Trading Name |
	| 1002  | My Only Name |                 |

	Given the following courses exist
	| Id   | Title                   | Level |
	| 9001 | Artificial Intelligence | 1     |
	| 9002 | Zoology                 | 3     |

Scenario: New apprenticeship is recieved and is valid 
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Email         | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Commitments Approved On |
	| 1                            | Test@Test.com | Apple         | 123                              | 1002                 | 2015-04-20  |
	Then the inner API has received the posted values
	And the Training Provider Name should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1
	And the invitation was sent successfully

Scenario: New apprenticeship is recieved and is valid and there is a trading name for provider
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Email              | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Commitments Approved On |
	| 2                            | SomeOther@Test.com | Apple         | 123                              | 1001                 | 2019-03-31  |
	Then the inner API has received the posted values
	And the Training Provider Name should be 'My Trading Name'
	And the invitation was sent successfully

Scenario: New apprenticeship is recieved and is Not valid 
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Email        | Employer Name | Employer Account Legal Entity Id | Training Provider Id |
	| 1                            | invalidemail | Apple         | 123                              | 1002                 |
	Then the inner API should return these errors
	| field | error     |
	| email | Not valid |
	And the invitation was not sent