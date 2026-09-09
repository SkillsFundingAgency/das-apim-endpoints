using SFA.DAS.RecruitQa.Domain.Enums;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public sealed record GetProfanityListApiRequest : IGetApiRequest
{
    public string GetUrl => $"api/prohibitedcontent/{ProhibitedContentType.Profanity}";
}