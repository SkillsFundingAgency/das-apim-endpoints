Feature: AddOrUpdateMyApprenticeship

As a Portal application 
I need to add or update my apprenticeship details 
when MyApprenticeshipConfirmedRequest is posted. 
We assume this confirmation cannot happen unless the apprentice exists

Scenario: The first confirmation message is processed successfully for provider with a trading name
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider exists 
	When the MyApprenticeshipConfirmedRequest is posted
	Then the call to add MyApprenticeship is called
	And it contains all the information
	And it contains the providers trading name
	And and response from API is Ok
	
Scenario: The first confirmation message is processed successfully for provider without a trading name
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider has no trading name
	And training provider exists 
	When the MyApprenticeshipConfirmedRequest is posted
	Then the call to add MyApprenticeship is called
	And it contains all the information
	And it contains the providers legal name
	And and response from API is Ok

Scenario: The second confirmation message is processed and overwrites the previous MyApprenticeship for provider with a trading name
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider exists 
	And MyApprenticeship exists 
	When the MyApprenticeshipConfirmedRequest is posted
	Then the call to update MyApprenticeship is called
	And it contains all the information
	And it contains the providers trading name
	And and response from API is Ok

Scenario: The confirmation message is processed but the GetMyApprenticeship does not return Ok or NotFound
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider exists 
	And MyApprenticeship returns an invalid status
	When the MyApprenticeshipConfirmedRequest is posted
	Then and response from API is InternalError
