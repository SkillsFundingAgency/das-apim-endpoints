@loginApi
Feature: ContinuationOfAnApprenticeship
	When an Apprenticeship is approved (as a Continuation of a previous apprenticeship) and forwarded here
	As an outer API
	I want to receive the details and call the change apprenticeship inner API

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Course Name             | Course Code | Continuation Of Id |
	| 1  | Alexa      | Armstrong | Artificial Intelligence | 9001        | 99                 |
	| 2  | Zachary    | Zimmerman | Zoology                 | 9002        | 100                |

	Given the following training providers exist
	| Ukprn | Legal Name   | Trading Name    |
	| 1001  | My Real Name | My Trading Name |
	| 1002  | My Only Name |                 |

	Given the following courses exist
	| Id   | Title                   | Level |
	| 9001 | Artificial Intelligence | 1     |
	| 9002 | Zoology                 | 3     |

Scenario: A continuation apprenticeship is recieved and is valid 
	When the following apprenticeship is posted
	| ApprenticeshipId | Email         | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Approved On |
	| 1                | Test@Test.com | Apple         | 123                              | 1002                 | 2015-04-20  |
	Then the inner API update endpoint has received the posted values
	And the Training Provider Name should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1
	And the invitation was not sent

	Scenario: A continuation apprenticeship is recieved and is valid and uses a different provider
	When the following apprenticeship is posted
	| ApprenticeshipId | Email          | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Approved On |
	| 2                | Test2@Test.com | Apple         | 123                              | 1001                 | 2015-04-20  |
	Then the inner API update endpoint has received the posted values
	And the Training Provider Name should be 'My Trading Name'
	And the course should be `Zoology` level 3
	And the invitation was not sent
