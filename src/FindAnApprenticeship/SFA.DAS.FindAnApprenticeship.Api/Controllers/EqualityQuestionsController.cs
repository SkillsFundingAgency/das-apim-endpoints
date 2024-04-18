using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class EqualityQuestionsController(IMediator mediator, ILogger<EqualityQuestionsController> logger)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostEqualityQuestionsApiRequest request)
    {
        try
        {
            var result = await mediator.Send(new UpsertAboutYouEqualityQuestionsCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                EthnicGroup = request.EthnicGroup,
                EthnicSubGroup = request.EthnicSubGroup,
                Sex = request.Sex,
                IsGenderIdentifySameSexAtBirth = request.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = request.OtherEthnicSubGroupAnswer,
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created($"{result.Id}", (PostEqualityQuestionsApiResponse)result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error posting equality questions application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}