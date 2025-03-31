using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public record GetDashboardByUkprnQuery(int Ukprn, ApplicationStatus Status) : IRequest<GetDashboardByUkprnQueryResult>;
}