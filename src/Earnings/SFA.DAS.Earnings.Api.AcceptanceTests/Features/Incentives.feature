Feature: Incentives

I want incentive payments to appear in the correct place in the FM36 block

Scenario: Incentives
	Given an apprentice aged 18 at the start of the apprenticeship
	And the following price episodes
	| StartDate  | EndDate    |
	| 2024-08-01 | 2025-07-31 |
	And the following instalments:
	| AcademicYear | DeliveryPeriod | Amount |
	| 2425         | 1              | 1500   |
	And the following additional payments:
	| Type              | AcademicYear | DeliveryPeriod | DueDate     | Amount |
	| EmployerIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| ProviderIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| EmployerIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	| ProviderIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                     | AcademicYear | Period | Value |
	| 1       | PriceEpisodeFirstEmp1618Pay   | 2425         | 3      | 500   |
	| 1       | PriceEpisodeFirstProv1618Pay  | 2425         | 3      | 500   |
	| 1       | PriceEpisodeSecondEmp1618Pay  | 2425         | 12     | 500   |
	| 1       | PriceEpisodeSecondProv1618Pay | 2425         | 12     | 500   |


