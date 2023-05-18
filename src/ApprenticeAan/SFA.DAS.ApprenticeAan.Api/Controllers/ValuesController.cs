using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IAanHubRestApiClient _restApiClient;

    public ValuesController(IAanHubRestApiClient restApiClient)
    {
        _restApiClient = restApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _restApiClient.Get<PingApiResponse>("values", new[] { new KeyValuePair<string, string>("X-RequestedByMemberId", "King Kong") });
        return Ok(response);
    }

    public class PingApiResponse
    {
        public string RequestedByMemberId { get; set; } = null!;
    }

    public class GetPingRequest : IGetApiRequest
    {
        public string GetUrl => "values";
    }
}
