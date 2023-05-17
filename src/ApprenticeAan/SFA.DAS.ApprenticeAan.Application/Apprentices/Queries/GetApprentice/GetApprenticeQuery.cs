using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;

public record GetApprenticeQuery(Guid ApprenticeId) : IRequest<GetApprenticeQueryResult?>;
