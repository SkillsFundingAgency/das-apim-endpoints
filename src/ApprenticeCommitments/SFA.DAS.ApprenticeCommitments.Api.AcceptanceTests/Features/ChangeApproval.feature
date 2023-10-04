@loginApi
Feature: ChangeApproval
	When an Apprenticeship is updated and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Background:
	Given the following apprenticeships have been approved
	| Id | First Name | Last Name | Date Of Birth | Course Name             | Course Code | ProviderId | AccountLegalEntityId | Employer Name | Email | Delivery Model   | Employment End Date | Option          | RecognisePriorLearning | DurationReducedByHours | DurationReducedBy |
	| 1  | Alexa      | Armstrong | 2000-01-01    | Artificial Intelligence | 9001        | 1001       | 8001                 | Apple         | a@a   | Regular          |                     | MachineLearning | true                   | 100                    | 10                |
	| 2  | Zachary    | Zimmerman | 2000-12-28    | Zoology                 | 9002        | 1002       | 8002                 | Google        | b@b   | PortableFlexiJob | 2022-02-09          | Primates        | false                  |                        |                   |
	| 3  | Gary       | Oldman    | 2000-12-28    | Zoology                 | 9002        | 1002       | 8002                 | Google        | b@b   | Regular          |                     | Insects         | false                  |                        |                   |
	| 4  | Zachary    | Zimmerman | 2004-04-19    | Zoology                 | 9002        | 1002       | 8002                 | Google        |       | Regular          |                     |                 | false                  |                        |                   |
	| 5  | Freddy     | Flintsone | 2004-04-19    | Framework Course        | 11-22-33    | 1002       | 8002                 | Google        | d@d   | Regular          |                     |                 | false                  |                        |                   |

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
	Then the response should be OK
	And the inner API has received the posted values
	And the Employer should be Legal Entity 8001 named 'Apple'
	And the Training Provider should be 'My Only Name'
	And the course should be `Artificial Intelligence` level 1 courseDuration 12
	And the delivery model should be "Regular"
	And the employment end date should be ""

Scenario: Apprenticeship update is recieved and is valid and there is a trading name for provider
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 2                            | 2015-04-20              |
	Then the response should be OK
	And the inner API has received the posted values
	And the Employer should be Legal Entity 8002 named 'Google'
	And the Training Provider should be 'My Trading Name'
	And the course should be `Zoology` level 3 courseDuration 13
	And the apprentice name should be 'Zachary' 'Zimmerman'
	And the apprentice date of Birth should be '2000-12-28'
	And the delivery model should be "PortableFlexiJob"
	And the employment end date should be "2022-02-09"

Scenario: Old apprenticeship update is recieved
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 3                            | 2015-04-20              |
	Then the response should be OK
	And the inner API has received the posted values
	And the delivery model should be "Regular"

Scenario: Apprenticeship update is recieved and is a continuation apprenticeship
	When the following apprenticeship update is posted
	| Commitments Continuation Of ApprenticeshipId | Commitments ApprenticeshipId | Commitments Approved On |
	| 1                                            | 2                            | 2015-04-20              |
	Then the response should be OK
	And the inner API has received the posted values

Scenario: Apprenticeship update is recieved but without an email
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 4                            | 2015-04-20              |
	Then the response should be OK
	And the inner API will not receive any values

Scenario: Framework apprenticeship update is received
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 5                            | 2015-04-20              |
	Then the response should be OK
	And the inner API will not receive any values

Scenario: Apprenticeship update is recieved and is valid with Rpl values
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 1                            | 2015-04-20              |
	Then the response should be OK
	And the inner API has received the posted values
	And recognise prior learning should be true with total hours reduction as 100 and weeks reduction 10

Scenario: Framework apprenticeship update is received but with no RPL values
	When the following apprenticeship update is posted
	| Commitments ApprenticeshipId | Commitments Approved On |
	| 3                            | 2015-04-20              |
	Then the response should be OK
	And there is no recognised prior learning values