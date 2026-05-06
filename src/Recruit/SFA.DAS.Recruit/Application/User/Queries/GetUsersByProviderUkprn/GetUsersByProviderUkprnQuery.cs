namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByProviderUkprn;

public sealed record GetUsersByProviderUkprnQuery(long Ukprn)
    : MediatR.IRequest<GetUsersByProviderUkprnQueryResult>;