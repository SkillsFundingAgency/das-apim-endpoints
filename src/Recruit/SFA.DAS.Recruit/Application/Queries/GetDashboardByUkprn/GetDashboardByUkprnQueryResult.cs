using SFA.DAS.Recruit.Application.Queries.Base;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn;

public record GetDashboardByUkprnQueryResult : DashboardBaseModel
{
    public static implicit operator GetDashboardByUkprnQueryResult(InnerApi.Responses.GetDashboardApiResponse response) =>
        Map(response, () => new GetDashboardByUkprnQueryResult());
}