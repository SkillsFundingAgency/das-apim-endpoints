@loginApi
Feature: ChangeApprenticeship
	When an Apprenticeship is updated and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Course Name             | Course Code | ProviderId | AccountLegalEntityId | Employer Name |
	| 1  | Alexa      | Armstrong | Artificial Intelligence | 9001        | 1001       | 8001                 | Apple         |
	| 2  | Zachary    | Zimmerman | Zoology                 | 9002        | 1002       | 8002                 | Google        |
	| 3  | Zachary    | Zimmerman | Zoology                 | 9002        | 1002       | 8002                 | Google        |

	Given the following training providers exist
	| Ukprn | Legal Name   | Trading Name    |
	| 1001  | My Only Name |                 |
	| 1002  | My Real Name | My Trading Name |

	Given the following courses exist
	| Id   | Title                   | Level |
	| 9001 | Artificial Intelligence | 1     |
	| 9002 | Zoology                 | 3     |

Scenario: Apprenticeship update is recieved and is valid
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 1                            | 2015-04-20              |
	Then the inner API has received the posted values
	And the Employer should be Legal Entity 8001 named 'Apple'
	And the Training Provider should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1

Scenario: Apprenticeship update is recieved and is valid and there is a trading name for provider
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 2                            | 2015-04-20              |
	Then the inner API has received the posted values
	And the Employer should be Legal Entity 8002 named 'Google'
	And the Training Provider should be 'My Trading Name'
	And the course should be `Zoology` level 3

Scenario: Apprenticeship update is recieved and is a continuation apprenticeship
	When the following apprenticeship update is posted
	| Commitments Continuation Of ApprenticeshipId | Commitments ApprenticeshipId | Commitments Approved On |
	| 1                                            | 3                            | 2015-04-20              |
	Then the inner API has received the posted values
