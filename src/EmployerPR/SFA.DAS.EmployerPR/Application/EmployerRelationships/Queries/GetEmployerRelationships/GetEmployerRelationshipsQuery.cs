using MediatR;

namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public record GetEmployerRelationshipsQuery(long AccountId) : IRequest<GetEmployerRelationshipsQueryResult>;
