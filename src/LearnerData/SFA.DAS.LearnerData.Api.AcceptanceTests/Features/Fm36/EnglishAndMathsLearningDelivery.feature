Feature: English and Maths Learning Delivery

Validate Learning Delivery for English And Maths

Scenario: Validate Number of Deliveries
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld onprogramme records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef |
		|              1627 | 2024-08-01 | 2025-07-31 | OpLrnA      |
	And has the following sld englishAndMaths records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef | Amount |
		|              1628 | 2024-08-01 | 2025-07-31 | EmLrnA      |   1300 |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then there are 2 learning deliveries
	And the aim sequence numbers on the learning deliveries are
		| AimSequenceNumber | LearnAimRef |
		|              1627 | OpLrnA      |
		|              1628 | EmLrnA      |
