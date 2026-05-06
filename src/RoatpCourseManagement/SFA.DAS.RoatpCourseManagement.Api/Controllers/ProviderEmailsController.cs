using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[Route("providers/{ukprn}/emails")]
[Tags("Providers")]
[ApiController]
public class ProviderEmailsController(IProviderAccountApiClient<ProviderAccountApiConfiguration> apiClient) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendEmailToProvider([FromRoute] int ukprn, [FromBody] ProviderEmailModel model, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<object>(new PostProviderEmailRequest(ukprn, model), false);
        response.EnsureSuccessStatusCode();
        return Ok();
    }
}
