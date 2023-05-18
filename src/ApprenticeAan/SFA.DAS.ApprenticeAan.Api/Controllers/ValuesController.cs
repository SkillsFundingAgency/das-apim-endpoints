using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

    public ValuesController(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _apiClient.GetWithResponseCode<PingApiResponse>(new GetPingRequest());
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
