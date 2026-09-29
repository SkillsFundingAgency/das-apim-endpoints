using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Requests.SldDataApi;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.Api.Controllers;

// TEMPORARY - FLP-1281 spike to prove the outer API can reach SLD's get-em-value endpoint from a real
// environment. Not part of the real solution: DELETE this controller (and the request class) before merge.
[Route("sld-data-api")]
[ApiController]
public class SldDataApiController(
    ISldDataApiClient<SLDDataApiConfiguration> sldDataApiClient,
    ILogger<SldDataApiController> logger) : ControllerBase
{
    [HttpGet("em-value/{learnAimRef}/{startDate}")]
    public async Task<IActionResult> GetEnglishAndMathsValue([FromRoute] string learnAimRef, [FromRoute] DateOnly startDate)
    {
        logger.LogInformation("SLD get-em-value test call: learnAimRef={LearnAimRef}, startDate={StartDate}", learnAimRef, startDate);

        try
        {
            var response = await sldDataApiClient.GetWithResponseCode<decimal?>(new GetEnglishAndMathsValueRequest(learnAimRef, startDate));

            logger.LogInformation("SLD get-em-value returned {StatusCode}", (int)response.StatusCode);

            // Pass SLD's status and body straight back so it's clear which layer failed (token, network block, SLD itself).
            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                ContentType = "text/plain",
                Content = response.Body?.ToString() ?? response.ErrorContent
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "SLD get-em-value test call failed");
            return StatusCode(StatusCodes.Status502BadGateway, $"{e.GetType().Name}: {e.Message}{(e.InnerException != null ? $" -> {e.InnerException.Message}" : "")}");
        }
    }
}
