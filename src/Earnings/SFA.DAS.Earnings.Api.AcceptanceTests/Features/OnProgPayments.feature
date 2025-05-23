Feature: OnProgPayments

I want on-programme payments to appear in PriceEpisodeOnProgPayment in the corresponding Price Episode in FM36 block

Scenario: OnProgrammePayments periodisation
Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2024-08-01 | 2025-07-31 |
	And the following instalments:
	| PriceEpisodeId | AcademicYear | DeliveryPeriod | Amount |
	| 1              | 2425         | 1              | 600    |
	| 1              | 2425         | 2              | 600    |
	| 1              | 2425         | 3              | 600    |
	| 1              | 2425         | 4              | 600    |
	| 1              | 2425         | 5              | 600    |
	| 1              | 2425         | 6              | 600    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                 | Period | Value |
	| 0       | PriceEpisodeOnProgPayment | 1      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 2      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 3      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 4      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 5      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 6      | 600   |
	And all other Price Episode PriceEpisodeOnProgPayment values are 0

