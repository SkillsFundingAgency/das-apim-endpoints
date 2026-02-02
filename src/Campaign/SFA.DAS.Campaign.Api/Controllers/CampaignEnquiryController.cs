#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CampaignEnquiryController(ILogger<CampaignEnquiryController> logger, ICampaignApiClient<CampaignApiConfiguration> apiClient) : ControllerBase
{
    // POST: CampaignEnquiryFormController/RegisterInterest
    [HttpPost("RegisterInterest")]
    [ProducesResponseType(typeof(EnquiryUserDataModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EnquiryUserDataModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RegisterInterest([FromBody] EnquiryUserDataModel userData)
    {
        try
        {
            var request = new PostRegisterInterestApiRequest(userData);
            var response = await apiClient.PostWithResponseCode<EnquiryUserDataModel>(request);

            return response.StatusCode switch
            {
                HttpStatusCode.Created => CreatedAtAction(nameof(RegisterInterest), response),
                HttpStatusCode.BadRequest => BadRequest(),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new InvalidOperationException("Campain Interest didn't come back with a successful response")
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving Campain Interest");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}


