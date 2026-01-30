using SFA.DAS.RecruitQa.Domain.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public sealed record GetProfanityListApiRequest : IGetApiRequest
{
    public string GetUrl => $"api/prohibitedcontent/{ProhibitedContentType.Profanity}";
}