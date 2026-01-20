using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.Models;

public sealed class GetRequest(string url) : IGetApiRequest
{
    public string GetUrl => url;
}