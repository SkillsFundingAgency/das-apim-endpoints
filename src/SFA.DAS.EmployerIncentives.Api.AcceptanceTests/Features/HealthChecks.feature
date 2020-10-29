@innerApi
@commitmentsV2InnerApi
@customerEngagementFinanceApi
@accountsApi
@outerApi
Feature: HealthChecks
	When a healthcheck is requested
	As an Outer Api
	I want return the status

Scenario Outline: Ping returns Ok 
	Given the Employer Incentives Inner Api is ready and Healthy
	And the Commitments Inner Api is ready and Healthy
	And the Accounts Api is ready and Healthy
	And the Finance Api is ready and Healthy
	When I ping the Outer Api
	Then the result should return Ok status

Scenario Outline: Ping returns Ok even when inner Apis are not
	Given the Employer Incentives Inner Api is ready and Unhealthy
	And the Commitments Inner Api is ready and Unhealthy
	And the Accounts Api is ready and Unhealthy
	And the Finance Api is ready and Unhealthy
	When I ping the Outer Api
	Then the result should return Ok status

Scenario Outline: HealthCheck is called
	Given the Employer Incentives Inner Api is ready and <InnerStatus>
	And the Commitments Inner Api is ready and <CommitmentsV2Status>
	And the Accounts Api is ready and <AccountsStatus>
	And the Finance Api is ready and <FinanceStatus>
	When I call the health endpoint of the Outer Api
	Then the result should show <OuterStatus>

Examples:
	| InnerStatus | CommitmentsV2Status | AccountsStatus | FinanceStatus | OuterStatus |
	| Healthy     | Healthy             | Healthy        | Healthy       | Healthy     |
	| Unhealthy   | Healthy             | Healthy        | Healthy       | Unhealthy   |
	| Healthy     | Unhealthy           | Healthy        | Healthy       | Unhealthy   |
	| Healthy     | Healthy             | Unhealthy      | Healthy       | Unhealthy   |
	| Healthy     | Healthy             | Healthy        | Unhealthy     | Unhealthy   |