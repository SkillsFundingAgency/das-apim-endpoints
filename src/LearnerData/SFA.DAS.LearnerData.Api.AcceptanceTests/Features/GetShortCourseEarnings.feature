Feature: GetShortCourseEarnings

Validates that short course earnings (the shortcourse equivilant of fm36) is returned

Scenario: When getting short course earnings pagination works (for first page)
	Given there are 35 short course learning records for ukprn 10000001 in academic year 2526 period 1
	When the get short course earnings endpoint is called for ukprn 10000001 in academic year 2526 period 1 for page 1 with page size 10
	Then the short course response should contain the following pagination details:
		| TotalRecords | TotalPages | PageNumber | PageSize | NumberOfRecordsInPage |
		| 35           | 4          | 1          | 10       | 10                    |

Scenario: When getting short course earnings pagination works (for last page)
	Given there are 35 short course learning records for ukprn 10000001 in academic year 2526 period 1
	When the get short course earnings endpoint is called for ukprn 10000001 in academic year 2526 period 1 for page 4 with page size 10
	Then the short course response should contain the following pagination details:
		| TotalRecords | TotalPages | PageNumber | PageSize | NumberOfRecordsInPage |
		| 35           | 4          | 4          | 10       | 5                     |

Scenario: When getting short course earnings data the return values are correctly mapped
	Given for ukprn 10000001 there are short course learning with
		| Price | IsApproved |
		| 1000  | true       |
		| 2000  | false      |
	And for ukprn 10000001 there are short course earnings with
		| CollectionYear1 | Period1 | Amount1 | Type1                         | CollectionYear2 | Period2 | Amount2 | Type2            |
		| 2526            | 5       | 300     | ThirtyPercentLearningComplete | 2526            | 12      | 700     | LearningComplete |
		| 2526            | 5       | 600     | ThirtyPercentLearningComplete | 2526            | 12      | 1400    | LearningComplete |
	When the get short course earnings endpoint is called for ukprn 10000001 in academic year 2526 period 1 for page 1 with page size 10
	Then the short course response should contain a record with the following details:
		| Price | IsApproved | CollectionYear1 | Period1 | Amount1 | Type1                         | CollectionYear2 | Period2 | Amount2 | Type2            |
		| 1000  | true       | 2526            | 5       | 300     | ThirtyPercentLearningComplete | 2526            | 12      | 700     | LearningComplete |
		| 2000  | false      | 2526            | 5       | 600     | ThirtyPercentLearningComplete | 2526            | 12      | 1400    | LearningComplete |