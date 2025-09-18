using SFA.DAS.Recruit.Application.Queries.Base;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;

public record GetDashboardByAccountIdQueryResult : DashboardBaseModel
{
    public static implicit operator GetDashboardByAccountIdQueryResult(InnerApi.Responses.GetDashboardApiResponse response) =>
        Map(response, () => new GetDashboardByAccountIdQueryResult());
}