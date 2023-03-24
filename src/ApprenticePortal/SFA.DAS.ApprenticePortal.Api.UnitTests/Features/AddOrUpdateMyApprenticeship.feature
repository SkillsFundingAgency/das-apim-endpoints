Feature: AddOrUpdateMyApprenticeship

As a Portal application 
I need to add or update my apprenticeship details 
when MyApprenticeshipConfirmedRequest is posted. 
We assume this confirmation cannot happen unless the apprentice exists

Scenario: The confirmation message is processed successfully for provider with a trading name
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider exists 
	When the MyApprenticeshipConfirmedRequest is posted
	Then the call to save MyApprenticeship is called
	And it contains all the information
	And it contains the providers trading name
	
Scenario: The confirmation message is processed successfully for provider without a trading name
	Given there is an apprentice
	And commitments apprenticeship exists 
	And training provider has no trading name
	And training provider exists 
	When the MyApprenticeshipConfirmedRequest is posted
	Then the call to save MyApprenticeship is called
	And it contains all the information
	And it contains the providers legal name