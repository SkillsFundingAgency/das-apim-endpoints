using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;
using System.Net;
using Microsoft.Extensions.Azure;
using SFA.DAS.LearnerData.Responses;
using Azure;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LearnersController(IMediator mediator, ILogger<LearnersController> logger) : ControllerBase
    {
        [HttpPut]
        [Route("/provider/{ukprn}/academicyears/{academicyear}/learners")]
        public async Task<IActionResult> Post([FromRoute]long ukprn, [FromRoute] int academicyear,  [FromBody] IEnumerable<LearnerDataRequest> dataRequests)
        {
            var errors = ValidateLearnerData(ukprn, dataRequests);
            if (errors.Any())
            {
                return new BadRequestObjectResult(new ErrorResponse { Errors = errors.ToList()});
            }

            try
            {
                var correlationId = Guid.NewGuid();
                await mediator.Send(new ProcessLearnersCommand { CorrelationId = correlationId, ReceivedOn  = DateTime.Now, AcademicYear = academicyear, Learners = dataRequests });
                return Accepted(new CorrelationResponse { CorrelationId = correlationId });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Internal error occurred when processing learners list");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        private IEnumerable<Error> ValidateLearnerData(long ukprn, IEnumerable<LearnerDataRequest> dataRequests)
        {
            if (dataRequests.Any(x => x.UKPRN != ukprn))
            {
                yield return new Error
                {
                    Code = "UKPRN",
                    Message = $"Learner data contains different UKPRN to {ukprn}"
                };
            }

            if (dataRequests.Any(x => x.ULN == 1000000000 || x.ULN == 9999999999999))
            {
                yield return new Error
                {
                    Code = "ULN",
                    Message = "Learner data contains incorrect ULNs"
                };

            }

        }



    }
}
