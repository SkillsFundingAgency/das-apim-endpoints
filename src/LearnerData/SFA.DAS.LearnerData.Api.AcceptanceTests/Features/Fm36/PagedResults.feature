Feature: PagedResults

This tests the paging functionality

Scenario: When query calls for 1st page in paged results
	Given there are 120 records in the system
	When I call the API with page size 20 and page number 1
	Then I receive 20 records
	And the following paging metadata is returned
		| TotalRecords | PageSize | PageNumber | TotalPages | HasNextPage | HasPreviousPage |
		| 120          | 20       | 1          | 6          | true        | false           |

Scenario: When query calls for 2nd page in paged results
	Given there are 120 records in the system
	When I call the API with page size 20 and page number 2
	Then I receive 20 records
	And the following paging metadata is returned
		| TotalRecords | PageSize | PageNumber | TotalPages | HasNextPage | HasPreviousPage |
		| 120          | 20       | 2          | 6          | true        | true            |

Scenario: When query calls for last page in paged results
	Given there are 120 records in the system
	When I call the API with page size 20 and page number 6
	Then I receive 20 records
	And the following paging metadata is returned
		| TotalRecords | PageSize | PageNumber | TotalPages | HasNextPage | HasPreviousPage |
		| 120          | 20       | 6          | 6          | false       | true            |

Scenario: When query calls for last page in paged results with a page size that does not evenly divide the total records
	Given there are 125 records in the system
	When I call the API with page size 20 and page number 7
	Then I receive 5 records
	And the following paging metadata is returned
		| TotalRecords | PageSize | PageNumber | TotalPages | HasNextPage | HasPreviousPage |
		| 125          | 20       | 7          | 7          | false       | true            |
