using MediatR;

namespace SFA.DAS.RecruitQa.Application.Provider.GetProvider;

public sealed record GetProviderQuery(int Ukprn) : IRequest<GetProviderQueryResult>;