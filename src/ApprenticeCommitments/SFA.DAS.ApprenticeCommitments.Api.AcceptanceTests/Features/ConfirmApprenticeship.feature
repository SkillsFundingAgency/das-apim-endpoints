Feature: Confirm Apprenticeship

Scenario Outline: Confirm an aspect of the apprenticeship
	Given the confirmation `<data>`
	And the inner API will accept the confirmation
	When we confirm the aspect
	Then return an ok response
	And confirm the details with the inner API

	Examples: 
	| data                                        |
	| {"ApprenticeshipDetailsCorrect": true}      |
	| {"EmployerCorrect": true}                   |
	| {"TrainingProviderCorrect": true}           |
	| {"RolesAndResponsibilitiesCorrect": true}   |
	| {"HowApprenticeshipDeliveredCorrect": true} |
	| {"ApprenticeshipCorrect": true}             |