Feature: ConfirmApprenticeshipFeature

Scenario Outline: Confirm an aspect of the apprenticeship
	Given <command> containing `<data>` for <confirmation>
	And the inner API will accept the confirmation
	When we confirm the aspect
	Then return an ok response
	And confirm the details with the inner API

	Examples: 
	| confirmation							| data										| command											|
	| ApprenticeshipDetailsConfirmation		| {"ApprenticeshipDetailsCorrect": true}	| ApprenticeshipDetailsConfirmationRequestData		|
	| EmployerConfirmation					| {"EmployerCorrect": true}					| EmployerConfirmationRequestData					|	
	| TrainingProviderConfirmation			| {"TrainingProviderCorrect": true}			| TrainingProviderConfirmationRequestData			|
	| RolesAndResponsibilitiesConfirmation  | {"RolesAndResponsibilitiesCorrect": true} | RolesAndResponsibilitiesConfirmationRequestData   |
