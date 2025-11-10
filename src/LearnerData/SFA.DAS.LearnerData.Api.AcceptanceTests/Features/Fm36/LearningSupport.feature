Feature: LearningSupport

I want Learning Support earnings to show in the FM36 periodised values under PriceEpisodeLSFCash

Scenario: Learning Support
	Given the following price episodes
	| PriceEpisodeId | StartDate  | EndDate    |
	| 1              | 2024-08-01 | 2025-07-31 |
	And the following additional payments:
	| Type            | AcademicYear | DeliveryPeriod | DueDate     | Amount |
	| LearningSupport | 2425         | 1              | 31-Aug-2024 | 150    |
	| LearningSupport | 2425         | 2              | 30-Sep-2024 | 150    |
	| LearningSupport | 2425         | 6              | 31-Oct-2024 | 150    |
	| LearningSupport | 2425         | 7              | 30-Nov-2024 | 150    |
	When the FM36 block is retrieved for Academic Year 2425 Delivery Period 1
	Then the Price Episode Periodised Values are as follows:
	| Episode | Attribute           | Period | Value |
	| 0       | PriceEpisodeLSFCash | 1      | 150   |
	| 0       | PriceEpisodeLSFCash | 2      | 150   |
	| 0       | PriceEpisodeLSFCash | 6      | 150   |
	| 0       | PriceEpisodeLSFCash | 7      | 150   |
	And all other Price Episode PriceEpisodeLSFCash values are 0
	And the Learning Delivery Periodised Values are as follows:
	| Episode | Attribute         | Period | Value |
	| 0       | LearnSuppFundCash | 1      | 150   |
	| 0       | LearnSuppFundCash | 2      | 150   |
	| 0       | LearnSuppFundCash | 6      | 150   |
	| 0       | LearnSuppFundCash | 7      | 150   |
	And all other Learning Delivery LearnSuppFundCash values are 0