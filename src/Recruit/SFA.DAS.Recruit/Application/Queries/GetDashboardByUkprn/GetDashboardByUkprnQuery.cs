using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public record GetDashboardByUkprnQuery(int Ukprn) : IRequest<GetDashboardByUkprnQueryResult>;
}