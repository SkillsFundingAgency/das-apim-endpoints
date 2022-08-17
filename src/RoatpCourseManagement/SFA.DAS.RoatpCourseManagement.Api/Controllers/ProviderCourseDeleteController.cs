using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseDeleteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseDeleteController> _logger;

        public ProviderCourseDeleteController(ILogger<ProviderCourseDeleteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/delete")]
        public async Task<IActionResult> DeleteProviderCourseLocation(int ukprn, int larsCode, DeleteProviderCourseCommand command)
        {
            _logger.LogInformation("Outer API: Request received to delete provider course for ukprn: {ukprn} larscode: {larscode} by UserId: {userId}", ukprn, larsCode, command.UserId);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
