Feature: FundStart and ThresholdDays

I want to set the FundStart and ThresholdDays correctly in the FM36 block
based upon whether the learner reached the qualifying period
Qualifying periods: >= 168 day planned duration: 42 days
					14 to 167 day planned duration: 14 days
					<14 day planned duration: 1 day

Scenario: FundStart defaults to True while learners are Active
	Given the following price episodes
	| PriceEpisodeId | StartDate    | EndDate    |
	| 1              | <start_date> | <end_date> |
	When the FM36 block is retrieved for Academic Year <academic_year> Delivery Period <delivery_period>
	Then FundStart for the Learning Delivery is true
	Examples: 
	| start_date | end_date   | academic_year | delivery_period |
	| 2020-01-01 | 2024-01-01 | 2021          | 1               | 
	| 2020-01-01 | 2020-02-01 | 2021          | 1               | 
	| 2020-01-01 | 2020-01-08 | 2021          | 1               |

Scenario: FundStart and ThresholdDays based on qualifying period
	Given the following price episodes
	| PriceEpisodeId | StartDate    | EndDate    |
	| 1              | <start_date> | <end_date> |
	And the learner withdraws on <withdrawal_date>
	When the FM36 block is retrieved for Academic Year <academic_year> Delivery Period <delivery_period>
	Then FundStart for the Learning Delivery is <expected_fundstart>
	Then ThresholdDays for the Learning Delivery is <expected_threshold_days>
	Examples: 
	| start_date | end_date   | withdrawal_date | academic_year | delivery_period | expected_fundstart | expected_threshold_days |
	| 2020-01-01 | 2024-01-01 | 2020-02-11      | 2021          | 1               | true               | 42                      |
	| 2020-01-01 | 2024-01-01 | 2020-02-10      | 2021          | 1               | false              | 42                      |
	| 2020-01-01 | 2020-02-01 | 2020-01-14      | 2021          | 1               | true               | 14                      |
	| 2020-01-01 | 2020-02-01 | 2020-01-13      | 2021          | 1               | false              | 14                      |
	| 2020-01-01 | 2020-01-08 | 2020-01-01      | 2021          | 1               | true               | 1                       |
