using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByUkprn;
public record GetAlertsByUkprnQuery(int Ukprn, string UserId) : IRequest<GetAlertsByUkprnQueryResult>;