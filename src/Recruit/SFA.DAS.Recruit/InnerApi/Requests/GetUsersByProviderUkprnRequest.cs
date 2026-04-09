using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public sealed record GetUsersByProviderUkprnRequest(long Ukprn): IGetApiRequest
{
    public string GetUrl => $"api/user/by/ukprn/{Ukprn}";
}