Feature: BreakInLearning

A short summary of the feature

@tag1
Scenario: No break in learning returns single learning delivery
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld onprogramme records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef  |
		| 1627              | 2024-08-01 | 2025-07-31 | TestLearnAim |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then there are 1 learning deliveries
	And the aim sequence numbers on the learning deliveries are
		| AimSequenceNumber | LearnAimRef |
		| 1627                 | TestLearnAim    |

Scenario: Has break in learning returns multiple learning deliveries
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld onprogramme records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef  |
		| 1627              | 2024-08-01 | 2025-01-31 | TestLearnAim |
		| 1628              | 2025-03-01 | 2025-07-31 | TestLearnAim |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then there are 2 learning deliveries
	And the aim sequence numbers on the learning deliveries are
		| AimSequenceNumber | LearnAimRef  |
		| 1627              | TestLearnAim |
		| 1628              | TestLearnAim |