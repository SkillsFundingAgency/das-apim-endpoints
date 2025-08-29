Feature: Incentives

I want incentive payments to appear in the correct place in the FM36 block

Scenario: Incentives in a single-year apprenticeship
	Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2024-08-01 | 2025-07-31 |
	And the following additional payments:
	| Type              | AcademicYear | DeliveryPeriod | DueDate     | Amount |
	| EmployerIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| ProviderIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| EmployerIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	| ProviderIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                     | Period | Value |
	| 0       | PriceEpisodeFirstEmp1618Pay   | 3      | 500   |
	| 0       | PriceEpisodeFirstProv1618Pay  | 3      | 500   |
	| 0       | PriceEpisodeSecondEmp1618Pay  | 12     | 500   |
	| 0       | PriceEpisodeSecondProv1618Pay | 12     | 500   |
	And all other Price Episode PriceEpisodeFirstEmp1618Pay values are 0
	And all other Price Episode PriceEpisodeSecondEmp1618Pay values are 0
	And the Learning Delivery Periodised Values are as follows:
	| Episode | Attribute                 | Period | Value |
	| 0       | LearnDelFirstEmp1618Pay   | 3      | 500   |
	| 0       | LearnDelFirstProv1618Pay  | 3      | 500   |
	| 0       | LearnDelSecondEmp1618Pay  | 12     | 500   |
	| 0       | LearnDelSecondProv1618Pay | 12     | 500   |
	And all other Learning Delivery LearnDelFirstEmp1618Pay values are 0
	And all other Learning Delivery LearnDelFirstProv1618Pay values are 0
	And all other Learning Delivery LearnDelSecondEmp1618Pay values are 0
	And all other Learning Delivery LearnDelSecondProv1618Pay values are 0

Scenario: Incentives in an apprenticeship with a price change
	Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2024-08-01 | 2025-01-14 |
	| 2              | 2025-01-15 | 2025-07-31 |
	And the following additional payments:
	| Type              | AcademicYear | DeliveryPeriod | DueDate     | Amount |
	| EmployerIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| ProviderIncentive | 2425         | 3              | 29-Oct-2024 | 500    |
	| EmployerIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	| ProviderIncentive | 2425         | 12             | 31-Jul-2025 | 500    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                     | Period | Value |
	| 0       | PriceEpisodeFirstEmp1618Pay   | 3      | 500   |
	| 0       | PriceEpisodeFirstProv1618Pay  | 3      | 500   |
	| 1       | PriceEpisodeSecondEmp1618Pay  | 12     | 500   |
	| 1       | PriceEpisodeSecondProv1618Pay | 12     | 500   |
	And all other Price Episode PriceEpisodeFirstEmp1618Pay values are 0
	And all other Price Episode PriceEpisodeSecondEmp1618Pay values are 0
	And the Learning Delivery Periodised Values are as follows:
	| Episode | Attribute                 | Period | Value |
	| 0       | LearnDelFirstEmp1618Pay   | 3      | 500   |
	| 0       | LearnDelFirstProv1618Pay  | 3      | 500   |
	| 0       | LearnDelSecondEmp1618Pay  | 12     | 500   |
	| 0       | LearnDelSecondProv1618Pay | 12     | 500   |
	And all other Learning Delivery LearnDelFirstEmp1618Pay values are 0
	And all other Learning Delivery LearnDelFirstProv1618Pay values are 0
	And all other Learning Delivery LearnDelSecondEmp1618Pay values are 0
	And all other Learning Delivery LearnDelSecondProv1618Pay values are 0

Scenario: Incentives in a multi-academic-year apprenticeship with a price change
	Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2025-03-01 | 2025-11-30 |
	| 2              | 2025-12-01 | 2026-02-28 |
	And the following additional payments:
	| Type              | AcademicYear | DeliveryPeriod | DueDate     | Amount |
	| EmployerIncentive | 2425         | 10             | 31-May-2025 | 500    |
	| ProviderIncentive | 2425         | 10             | 31-May-2025 | 500    |
	| EmployerIncentive | 2526         | 7              | 28-Feb-2026 | 500    |
	| ProviderIncentive | 2526         | 7              | 28-Feb-2026 | 500    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 12
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                    | Period | Value |
	| 0       | PriceEpisodeFirstEmp1618Pay  | 10     | 500   |
	| 0       | PriceEpisodeFirstProv1618Pay | 10     | 500   |
	And all other Price Episode PriceEpisodeFirstEmp1618Pay values are 0
	And all other Price Episode PriceEpisodeSecondEmp1618Pay values are 0
	And the Learning Delivery Periodised Values are as follows:
	| Episode | Attribute                | Period | Value |
	| 0       | LearnDelFirstEmp1618Pay  | 10     | 500   |
	| 0       | LearnDelFirstProv1618Pay | 10     | 500   |
	And all other Learning Delivery LearnDelFirstEmp1618Pay values are 0
	And all other Learning Delivery LearnDelFirstProv1618Pay values are 0
	When the FM36 block is retrieved for Academic Year 2526 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute                     | Period | Value |
	| 1       | PriceEpisodeSecondEmp1618Pay  | 7      | 500   |
	| 1       | PriceEpisodeSecondProv1618Pay | 7      | 500   |
	And all other Price Episode PriceEpisodeFirstEmp1618Pay values are 0
	And all other Price Episode PriceEpisodeSecondEmp1618Pay values are 0
	And the Learning Delivery Periodised Values are as follows:
	| Episode | Attribute                | Period | Value |
	| 0       | LearnDelSecondEmp1618Pay  | 7     | 500   |
	| 0       | LearnDelSecondProv1618Pay | 7     | 500   |
	And all other Learning Delivery LearnDelSecondEmp1618Pay values are 0
	And all other Learning Delivery LearnDelSecondProv1618Pay values are 0