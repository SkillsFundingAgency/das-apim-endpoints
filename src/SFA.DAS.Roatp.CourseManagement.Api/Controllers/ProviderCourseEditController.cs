using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateConfirmRegulatedStandard;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateContactDetails;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseEditController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseEditController> _logger;

        public ProviderCourseEditController(IMediator mediator, ILogger<ProviderCourseEditController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/update-contact-details")]
        public async Task<IActionResult> UpdateProviderCourseContactDetails(int ukprn, int larsCode, UpdateContactDetailsCommand command)
        {
            _logger.LogInformation("Outer API: Request to update course contact details for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            var httpStatusCode = await _mediator.Send(command);
            if (httpStatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Outer API: Failed request to update course contact details for ukprn: {ukprn} larscode: {larscode} with HttpStatusCode: {httpstatuscode}", ukprn, larsCode, httpStatusCode);
                return new StatusCodeResult((int)httpStatusCode);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/update-confirm-regulated-standard")]
        public async Task<IActionResult> UpdateProviderCourseConfirmRegulatedStandard(int ukprn, int larsCode, UpdateConfirmRegulatedStandardCommand command)
        {
            _logger.LogInformation("Outer API: Request to update confirm regulated standard for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            var httpStatusCode = await _mediator.Send(command);
            if (httpStatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Outer API: Failed request to update update confirm regulated standard for ukprn: {ukprn} larscode: {larscode} with HttpStatusCode: {httpstatuscode}", ukprn, larsCode, httpStatusCode);
                return new StatusCodeResult((int)httpStatusCode);
            }
            return NoContent();
        }
    }
}
