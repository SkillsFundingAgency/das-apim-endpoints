@innerApi
@commitmentsV2InnerApi
@outerApi
Feature: HealthChecks
	When a healthcheck is requested
	As an Outer Api
	I want return the status

Scenario Outline: Ping HealthCheck is called
	Given the Employer Incentives Inner Api is ready and <InnerStatus>
	And the Commitments Inner Api is ready and <CommitmentsV2Status>
	When I ping the Outer Api
	Then the result should show <OuterStatus>
Examples:
	| InnerStatus | CommitmentsV2Status | OuterStatus |
	| Healthy     | Healthy             | Healthy     |
	| Healthy     | Degraded            | Degraded    |
	| Degarded    | Degraded            | Unhealthy   |
	| Unhealthy   | Healthy             | Unhealthy   |
	| Healthy     | Unhealthy           | Unhealthy   |
	| Unhealthy   | Degraded            | Unhealthy   |
	| Degraded    | Unhealthy           | Unhealthy   |
