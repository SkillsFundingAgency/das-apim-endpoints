using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using System.Net;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShortCoursesController(
        IMediator mediator,
        ILogger<ShortCoursesController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("/providers/{ukprn}/shortCourses")]
        public async Task<IActionResult> CreateShortCourse(ShortCourseRequest request, [FromQuery] long ukprn)
        {
            try
            {
                await mediator.Send(new CreateDraftShortCourseCommand
                {
                    Ukprn = ukprn,
                    ShortCourseRequest = request
                });
                return Accepted();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Internal error occurred when creating short course");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
