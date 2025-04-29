using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public record GetDashboardByUkprnQuery(int Ukprn, ApplicationReviewStatus Status) : IRequest<GetDashboardByUkprnQueryResult>;
}