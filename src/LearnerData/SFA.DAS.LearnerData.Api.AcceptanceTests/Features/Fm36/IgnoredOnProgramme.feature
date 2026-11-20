Feature: IgnoredOnProgramme

The SLD cache holds every on programme aim for a learner, but only the aims for the same standard and agreement
as the earliest aim (the first apprenticeship) are sent to Learning and earned on. Any other aims are ignored and must not be returned
as learning deliveries, nor take the earnings of the first apprenticeship aims.

Scenario: On programme aim for another agreement is not returned as a learning delivery
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And the following instalments:
		| PriceEpisodeId | AcademicYear | DeliveryPeriod | Amount | InstalmentType |
		| 1              | 2425         | 1              | 600    | Regular        |
		| 1              | 2425         | 2              | 600    | Regular        |
		| 1              | 2425         | 3              | 600    | Regular        |
		| 1              | 2425         | 4              | 600    | Regular        |
		| 1              | 2425         | 5              | 600    | Regular        |
		| 1              | 2425         | 6              | 600    | Regular        |
	And has the following sld onprogramme records
		| AimSequenceNumber | StartDate  | ExpectedEndDate | LearnAimRef  | AgreementId |
		| 4                 | 2024-08-01 | 2025-07-31      | TestLearnAim | Agreement1  |
		| 5                 | 2024-10-01 | 2025-07-31      | TestLearnAim | Agreement2  |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then there are 1 learning deliveries
	And the aim sequence numbers on the learning deliveries are
		| AimSequenceNumber | LearnAimRef  |
		| 4                 | TestLearnAim |

Scenario: On programme aim for another standard is not returned as a learning delivery
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And the following instalments:
		| PriceEpisodeId | AcademicYear | DeliveryPeriod | Amount | InstalmentType |
		| 1              | 2425         | 1              | 600    | Regular        |
		| 1              | 2425         | 2              | 600    | Regular        |
		| 1              | 2425         | 3              | 600    | Regular        |
	And has the following sld onprogramme records
		| AimSequenceNumber | StartDate  | ExpectedEndDate | LearnAimRef  | StandardCode |
		| 4                 | 2024-08-01 | 2025-07-31      | TestLearnAim | 100          |
		| 5                 | 2024-10-01 | 2025-07-31      | OtherAim     | 200          |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then there are 1 learning deliveries
	And the aim sequence numbers on the learning deliveries are
		| AimSequenceNumber | LearnAimRef  |
		| 4                 | TestLearnAim |
