using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
public record GetStagedApprenticeQuery(string LastName, DateTime DateOfBirth, string Email) : IRequest<GetStagedApprenticeQueryResult?>;
