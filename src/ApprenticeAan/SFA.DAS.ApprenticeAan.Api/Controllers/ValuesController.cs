using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IInnerApiRestClient _restApiClient;

    public ValuesController(IInnerApiRestClient restApiClient)
    {
        _restApiClient = restApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromHeader] string header)
    {
        var response = await _restApiClient.GetValues(header);
        return Ok(response.GetContent());
    }

    public class GetPingRequest : IGetApiRequest
    {
        public string GetUrl => "values";
    }
}

