using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.Api.Models;

public sealed class GetRequest(string url) : IGetApiRequest
{
    public string GetUrl => url;
}