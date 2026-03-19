namespace SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfileByLegalEntityId;

public sealed record GetEmployerProfileByLegalEntityIdQuery(long AccountLegalEntityId)
    : MediatR.IRequest<GetEmployerProfileByLegalEntityIdQueryResult>;