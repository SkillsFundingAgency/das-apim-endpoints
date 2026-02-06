#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CampaignEnquiryController(ILogger<CampaignEnquiryController> logger, ICampaignApiClient apiClient) : ControllerBase
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
            logger.LogInformation("Register Campaign Interest Outer API: Received request to add user details to campaign");

            var request = new PostRegisterInterestApiRequest(userData);
            var response = await apiClient.PostWithResponseCode<EnquiryUserDataModel>(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    {
                        logger.LogInformation("Register Campaign Interest Outer API: Successfully added user details to campaign");
                        return CreatedAtAction(nameof(RegisterInterest), response);
                    }
                case HttpStatusCode.BadRequest:
                    {
                        logger.LogWarning("Register Campaign Interest Outer API received Bad request from Inner API");
                        return BadRequest();
                    }
                case HttpStatusCode.InternalServerError:
                    {
                        logger.LogError("Register Campaign Interest Outer API received Internal server error from Inner API");
                        return StatusCode((int)HttpStatusCode.InternalServerError);
                    }
                default:
                    {
                        throw new InvalidOperationException("Campaign Interest didn't receive a successful response from Inner API");
                    }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to register Campaign Interest");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}


