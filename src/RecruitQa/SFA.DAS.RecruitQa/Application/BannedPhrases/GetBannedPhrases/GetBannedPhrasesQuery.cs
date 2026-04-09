using MediatR;

namespace SFA.DAS.RecruitQa.Application.BannedPhrases.GetBannedPhrases;

public sealed record GetBannedPhrasesQuery : IRequest<GetBannedPhrasesQueryResult>;