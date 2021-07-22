@loginApi
Feature: ChangeApprenticeship
	When an Apprenticeship is updated and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Date Of Birth | Course Name             | Course Code | ProviderId | AccountLegalEntityId | Employer Name | Email |
	| 1  | Alexa      | Armstrong | 2000-01-01    | Artificial Intelligence | 9001        | 1001       | 8001                 | Apple         | a@a   |
	| 2  | Zachary    | Zimmerman | 2000-12-28    | Zoology                 | 9002        | 1002       | 8002                 | Google        | b@b   |
	| 3  | Zachary    | Zimmerman | 2001-03-03    | Zoology                 | 9002        | 1002       | 8002                 | Google        | c@c   |
	| 4  | Zachary    | Zimmerman | 2004-04-19    | Zoology                 | 9002        | 1002       | 8002                 | Google        |       |

	Given the following training providers exist
	| Ukprn | Legal Name   | Trading Name    |
	| 1001  | My Only Name |                 |
	| 1002  | My Real Name | My Trading Name |

	Given the following courses exist
	| Id   | Title                   | Level | CourseDuration |
	| 9001 | Artificial Intelligence | 1     | 12			  |
	| 9002 | Zoology                 | 3     | 13			  |

Scenario: Apprenticeship update is recieved and is valid
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 1                            | 2015-04-20              |
	Then the inner API has received the posted values
	And the Employer should be Legal Entity 8001 named 'Apple'
	And the Training Provider should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1 courseDuration 12

Scenario: Apprenticeship update is recieved and is valid and there is a trading name for provider
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 2                            | 2015-04-20              |
	Then the inner API has received the posted values
	And the Employer should be Legal Entity 8002 named 'Google'
	And the Training Provider should be 'My Trading Name'
	And the course should be `Zoology` level 3 courseDuration 13
	And the apprentice name should be 'Zachary' 'Zimmerman'
	And the apprentice date of Birth should be '2000-12-28'

Scenario: Apprenticeship update is recieved and is a continuation apprenticeship
	When the following apprenticeship update is posted
	| Commitments Continuation Of ApprenticeshipId | Commitments ApprenticeshipId | Commitments Approved On |
	| 1                                            | 3                            | 2015-04-20              |
	Then the inner API has received the posted values

Scenario: Apprenticeship update is recieved but without an email
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 4                            | 2015-04-20              |
	Then the inner API will not receive any values