using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using System.Net;
using SFA.DAS.LearnerData.Application.ProcessLearners;
using SFA.DAS.LearnerData.Application.RemoveLearner;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class LearnersController(
    IMediator mediator, 
    IValidator<CreateLearnerRequest> validator,
    IValidator<IEnumerable<LearnerDataRequest>> originalValidator,
    ILogger<LearnersController> logger) : ControllerBase
{
    [HttpGet("providers/{ukprn}/academicyears/{academicyear}/learners")]
    public async Task<IActionResult> GetLearners([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        logger.LogInformation("GetLearners for ukprn {Ukprn}, year {Year}", ukprn, academicyear);

        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 1, 100) : pagesize;

        var query = new GetLearnersQuery()
        {
            Ukprn = ukprn,
            AcademicYear = academicyear,
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);
        HttpContext.SetPageLinksInResponseHeaders(query, response);

        return Ok((GetLearnersResponse)response);
    }

    [HttpPut]
    [Route("/provider/{ukprn}/academicyears/{academicyear}/learners")]
    public async Task<IActionResult> Put([FromRoute] long ukprn, [FromRoute] int academicyear,
        [FromBody] IEnumerable<LearnerDataRequest> dataRequests)
    {

        var validatorResult = await originalValidator.ValidateAsync(dataRequests);

        if (!validatorResult.IsValid)
        {
            return BuildErrorResponse(validatorResult.Errors);
        }

        try
        {
            var correlationId = Guid.NewGuid();
            await mediator.Send(new ProcessLearnersCommand
            {
                CorrelationId = correlationId,
                ReceivedOn = DateTime.Now,
                AcademicYear = academicyear,
                Learners = dataRequests
            });
            return Accepted(new CorrelationResponse { CorrelationId = correlationId });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when processing learners list");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("/providers/{ukprn}/learners")]
    public async Task<IActionResult> CreateLearningRecord([FromRoute] long ukprn, [FromBody] CreateLearnerRequest dataRequest)
    {

        var validatorResult = await validator.ValidateAsync(dataRequest);

        if (!validatorResult.IsValid)
        {
            return BuildErrorResponse(validatorResult.Errors);
        }

        try
        {
            var correlationId = Guid.NewGuid();
            await mediator.Send(new CreateLearnerCommand
            {
                CorrelationId = correlationId, 
                ReceivedOn = DateTime.Now, 
                Request = dataRequest,
                Ukprn = ukprn
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
    [Route("/providers/{ukprn}/learning/{learningKey}")]
    public async Task<IActionResult> UpdateLearner([FromRoute] long ukprn, [FromRoute] Guid learningKey, [FromBody] UpdateLearnerRequest request)
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

    [HttpDelete("/providers/{ukprn}/learning/{learningKey}")]
    public async Task<IActionResult> RemoveLearner(
        [FromRoute] long ukprn,
        [FromRoute] Guid learningKey)
    {
        logger.LogInformation(
            "RemoveLearner for provider {ukprn}, apprenticeship {learningKey}",
            ukprn,
            learningKey);

        try
        {
            var command = new RemoveLearnerCommand
            {
                LearningKey = learningKey,
                Ukprn = ukprn
            };

            await mediator.Send(command);

            return NoContent();

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Internal error occurred when removing learner {learningKey}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Gets all earnings data.
    /// </summary>
    /// <returns>All earnings data in the format of an FM36Learner array.</returns>
    [HttpGet]
    [Route("providers/{ukprn}/collectionPeriod/{collectionYear}/{collectionPeriod}/fm36data")]
    public async Task<IActionResult> GetFm36Learners(long ukprn, int collectionYear, byte collectionPeriod)
    {
        try
        {
            var queryResult = await mediator.Send(new GetFm36Command(ukprn, collectionYear, collectionPeriod));

            var model = queryResult.FM36Learners;

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get all earnings");
            return BadRequest();
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