using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseLocationDeleteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseLocationDeleteController> _logger;

        public ProviderCourseLocationDeleteController(ILogger<ProviderCourseLocationDeleteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/bulk-delete-course-locations")]
        public async Task<IActionResult> BulkDeleteProviderCourseLocations(int ukprn, int larsCode, BulkDeleteProviderCourseLocationsCommand command)
        {
            _logger.LogInformation("Outer API: Request received to bulk delete provider course locations for ukprn: {ukprn} larscode: {larscode} option: {deleteOption}", ukprn, larsCode, command.DeleteProviderCourseLocationOption);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete]
        [Route("providers/{ukprn}/courses/{larsCode}/delete-providercourselocation/{id}")]
        public async Task<IActionResult> DeleteProviderCourseLocation(int ukprn, int larsCode, int id, DeleteProviderCourseLocationCommand command)
        {
            _logger.LogInformation("Outer API: Request received to delete provider course location for ukprn: {ukprn} larscode: {larscode} providerCourseLocationId: {id}", ukprn, larsCode, id);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
