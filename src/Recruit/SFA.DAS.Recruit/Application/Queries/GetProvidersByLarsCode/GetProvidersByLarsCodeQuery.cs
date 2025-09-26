using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;

public record GetProvidersByLarsCodeQuery(int LarsCode): IRequest<GetProvidersByLarsCodeQueryResult>;