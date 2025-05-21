using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;

public record GetStandardsQuery() : IRequest<GetStandardsQueryResult>;
