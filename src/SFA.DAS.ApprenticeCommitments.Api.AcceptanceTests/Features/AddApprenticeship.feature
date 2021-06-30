@loginApi
Feature: AddApprenticeship
	When an Apprenticeship is approved and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Course Name             | Course Code | StandardUId | Email             |
	| 1  | Alexa      | Armstrong | Artificial Intelligence | 9001        |             | alexa@example.org |
	| 3  | Iris       | Ignored   | Not Whitelisted         | 9003        |             |                   |
	| 4  | Simon      | Standard  | Sociology               |             | SOC191_1.0  | simon@example.org |
	| 2  | Zachary    | Zimmerman | Zoology                 | 9002        |             | zach@example.org  |

	Given the following training providers exist
	| Ukprn | Legal Name   | Trading Name    |
	| 1001  | My Real Name | My Trading Name |
	| 1002  | My Only Name |                 |

	Given the following courses exist
	| Id   | Title                   | Level | StandardUId |
	| 9001 | Artificial Intelligence | 1     |             |
	| 9002 | Zoology                 | 3     |             |
	| 9003 | Not Whitelisted         | 2     |             |
	| 9004 | Sociology               | 2     | SOC191_1.0  |

Scenario: New apprenticeship is recieved and is valid 
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Commitments Approved On |
	| 1                            | Apple         | 123                              | 1002                 | 2015-04-20  |
	Then the inner API has received the posted values
	And the Training Provider Name should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1
	And the invitation was sent successfully

Scenario: New apprenticeship is recieved and is valid and there is a trading name for provider
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Commitments Approved On |
	| 2                            | Apple         | 123                              | 1001                 | 2019-03-31  |
	Then the inner API has received the posted values
	And the Training Provider Name should be 'My Trading Name'
	And the invitation was sent successfully

Scenario: New apprenticeship is recieved for non-whitelisted approval
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Employer Name | Employer Account Legal Entity Id | Training Provider Id |
	| 3                            | SmallCo       | 124                              | 1002                 |
	Then the request should be ignored
	And the invitation was not sent

Scenario: New apprenticeship is recieved with Standards Versioning
	When the following apprenticeship is posted
	| Commitments ApprenticeshipId | Employer Name | Employer Account Legal Entity Id | Training Provider Id | Commitments Approved On |
	| 4                            | Irrelevant    | 123                              | 1002                 | 2015-04-20              |
	Then the course should be `Sociology` level 2

