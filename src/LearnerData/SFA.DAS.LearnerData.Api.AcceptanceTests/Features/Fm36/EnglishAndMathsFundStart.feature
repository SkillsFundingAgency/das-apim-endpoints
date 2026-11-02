Feature: English and Maths FundStart and ThresholdDays

I want to set the FundStart and ThresholdDays correctly in the FM36 block for English and Maths learning deliveries
based upon the English and Maths course's own dates and whether the learner reached the qualifying period
Qualifying periods: >= 168 day planned duration: 42 days
					14 to 167 day planned duration: 14 days
					<14 day planned duration: 1 day

Scenario: English and Maths FundStart defaults to True when not withdrawn
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld englishAndMaths records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef | Amount |
		| 1628              | 2024-08-01 | 2025-07-31 | EmLrnA      | 1300   |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then FundStart for the English and Maths Learning Delivery is true
	And ThresholdDays for the English and Maths Learning Delivery is 42

Scenario: English and Maths FundStart and ThresholdDays based on qualifying period
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld englishAndMaths records
		| AimSequenceNumber | StartDate    | EndDate    | LearnAimRef | Amount | WithdrawalDate    |
		| 1628              | <start_date> | <end_date> | EmLrnA      | 1300   | <withdrawal_date> |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then FundStart for the English and Maths Learning Delivery is <expected_fundstart>
	And ThresholdDays for the English and Maths Learning Delivery is <expected_threshold_days>
	Examples:
		| start_date | end_date   | withdrawal_date | expected_fundstart | expected_threshold_days |
		| 2024-08-01 | 2025-07-31 | 2024-09-11      | true               | 42                      |
		| 2024-08-01 | 2025-07-31 | 2024-09-10      | false              | 42                      |
		| 2024-08-01 | 2025-07-31 | 2025-03-01      | true               | 42                      |
		| 2024-08-01 | 2024-08-31 | 2024-08-14      | true               | 14                      |
		| 2024-08-01 | 2024-08-31 | 2024-08-13      | false              | 14                      |
		| 2024-08-01 | 2024-08-05 | 2024-08-01      | true               | 1                       |

Scenario: English and Maths ThresholdDays boundaries between planned durations
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And has the following sld englishAndMaths records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef | Amount |
		| 1628              | 2024-08-01 | <end_date> | EmLrnA      | 1300   |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then ThresholdDays for the English and Maths Learning Delivery is <expected_threshold_days>
	Examples:
		| end_date   | planned_days | expected_threshold_days |
		| 2024-08-13 | 13           | 1                       |
		| 2024-08-14 | 14           | 14                      |
		| 2025-01-14 | 167          | 14                      |
		| 2025-01-15 | 168          | 42                      |

Scenario: English and Maths qualifying period uses its own dates and not the on programme withdrawal
	Given the following price episodes
		| PriceEpisodeId | StartDate  | EndDate    |
		| 1              | 2024-08-01 | 2025-07-31 |
	And the learner withdraws on 2024-08-10
	And has the following sld englishAndMaths records
		| AimSequenceNumber | StartDate  | EndDate    | LearnAimRef | Amount | WithdrawalDate |
		| 1628              | 2024-08-01 | 2025-07-31 | EmLrnA      | 1300   | 2024-09-11     |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then FundStart for the English and Maths Learning Delivery is true
