using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;

namespace SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
public record GetStagedApprenticeQuery(string LastName, DateTime DateOfBirth, string Email) : IRequest<GetStagedApprenticeResponse?>;
