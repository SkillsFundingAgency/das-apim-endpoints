Feature: OnProgPayments

I want on-programme payments to appear in PriceEpisodeOnProgPayment in the corresponding Price Episode in FM36 block

Scenario: OnProgrammePayments periodisation
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

Scenario: OnProgrammePayments periodisation with price change
	Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2024-08-01 | 2025-01-14 |
	| 2              | 2025-01-15 | 2025-07-31 |
	And the following instalments:
	| PriceEpisodeId | AcademicYear | DeliveryPeriod | Amount | InstalmentType |
	| 1              | 2425         | 1              | 600    | Regular        |
	| 1              | 2425         | 2              | 600    | Regular        |
	| 1              | 2425         | 3              | 600    | Regular        |
	| 1              | 2425         | 4              | 600    | Regular        |
	| 1              | 2425         | 5              | 600    | Regular        |
	| 2              | 2425         | 6              | 600    | Regular        |
	| 2              | 2425         | 7              | 600    | Regular        |
	| 2              | 2425         | 8              | 600    | Regular        |
	| 2              | 2425         | 9              | 600    | Regular        |
	| 2              | 2425         | 10             | 600    | Regular        |
	| 2              | 2425         | 11             | 600    | Regular        |
	| 2              | 2425         | 12             | 600    | Regular        |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                 | Period | Value |
	| 0       | PriceEpisodeOnProgPayment | 1      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 2      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 3      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 4      | 600   |
	| 0       | PriceEpisodeOnProgPayment | 5      | 600   |
	| 1       | PriceEpisodeOnProgPayment | 6      | 600   |
	| 1       | PriceEpisodeOnProgPayment | 7      | 600   |
	| 1       | PriceEpisodeOnProgPayment | 8      | 600   |
	| 1       | PriceEpisodeOnProgPayment | 9      | 600   |
	| 1       | PriceEpisodeOnProgPayment | 10     | 600   |
	| 1       | PriceEpisodeOnProgPayment | 11     | 600   |
	| 1       | PriceEpisodeOnProgPayment | 12     | 600   |
	And all other Price Episode PriceEpisodeOnProgPayment values are 0