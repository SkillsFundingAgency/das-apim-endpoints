using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.ProcessLearners;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using System.Net;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class LearnersController(IMediator mediator, IValidator<IEnumerable<LearnerDataRequest>> validator, ILogger<LearnersController> logger) : ControllerBase
{
    [HttpPut]
    [Route("/provider/{ukprn}/academicyears/{academicyear}/learners")]
    public async Task<IActionResult> Put([FromRoute] long ukprn, [FromRoute] int academicyear,
        [FromBody] IEnumerable<LearnerDataRequest> dataRequests)
    {

        var validatorResult = await validator.ValidateAsync(dataRequests);

        if (!validatorResult.IsValid)
        {
            return BuildErrorResponse(validatorResult.Errors);
        }

        try
        {
            var correlationId = Guid.NewGuid();
            await mediator.Send(new ProcessLearnersCommand
            {
                CorrelationId = correlationId, ReceivedOn = DateTime.Now, AcademicYear = academicyear,
                Learners = dataRequests
            });
            return Accepted(new CorrelationResponse {CorrelationId = correlationId});
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when processing learners list");
            return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("{learningKey}")]
    public async Task<IActionResult> UpdateLearner([FromRoute] Guid learningKey, 
    [FromBody] UpdateLearnerRequest request)
    {
        try
        {
            await mediator.Send(new UpdateLearnerCommand
            {
                LearningKey = learningKey,
                UpdateLearnerRequest = request
            });
            return Accepted();
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Internal error occurred when updating learner {learningKey}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }


    private IActionResult BuildErrorResponse(List<ValidationFailure> errors)
    {
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
        return BadRequest(ModelState);

    }
}