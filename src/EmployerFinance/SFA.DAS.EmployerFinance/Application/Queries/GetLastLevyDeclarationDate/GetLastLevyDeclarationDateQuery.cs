namespace SFA.DAS.EmployerFinance.Application.Queries.GetLastLevyDeclarationDate;

public sealed record GetLastLevyDeclarationDateQuery(long AccountId)
    : MediatR.IRequest<GetLastLevyDeclarationDateQueryResult>;