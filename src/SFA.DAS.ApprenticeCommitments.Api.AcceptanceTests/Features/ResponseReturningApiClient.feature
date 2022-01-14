@innerApi
Feature: ResponseReturningApiClient

Scenario: Reminder sent
	When the outer api recieves a request to '/registrations/29368420-57cb-4364-88bd-45683e2ec984/reminder' with body
	"""
	{
		"SentOn": "2022-01-11T08:59:36.5143850"
	}
	"""
	Then the outer api forwards the request to the inner api
	
Scenario: Registration first seen
	When the outer api recieves a request to '/registrations/29368420-57cb-4364-88bd-45683e2ec984/firstseen' with body
	"""
	{
		"SeenOn": "2022-01-11T08:59:36.5143850"
	}
	"""
	Then the outer api forwards the request to the inner api