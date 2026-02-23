using MediatR;

namespace SFA.DAS.RecruitQa.Application.Profanity.GetProfanity;

public sealed record GetProfanityListQuery : IRequest<GetProfanityListQueryResult>;