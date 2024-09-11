using MediatR;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public record GetEmployerRelationshipsQuery(long AccountId) : IRequest<GetEmployerRelationshipsQueryResult>;
