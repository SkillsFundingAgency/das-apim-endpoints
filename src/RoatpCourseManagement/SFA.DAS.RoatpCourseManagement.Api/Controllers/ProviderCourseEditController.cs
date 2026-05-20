using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateOnlineDeliveryOption;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Infrastructure;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Courses")]
[Route("")]
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
    public async Task<IActionResult> UpdateProviderCourseContactDetails(int ukprn, string larsCode, UpdateContactDetailsCommand command)
    {
        _logger.LogInformation("Outer API: Request to update course contact details for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        try
        {
            await _mediator.Send(command);
        }
        catch (HttpRequestContentException ex)
        {
            _logger.LogError(ex, "Outer API: Failed request to update course contact details for ukprn: {Ukprn} larscode: {Larscode} with HttpStatusCode: {HttpStatusCode}", ukprn, larsCode, ex.StatusCode);
            return new StatusCodeResult((int)ex.StatusCode);
        }
        return NoContent();
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/update-approved-by-regulator")]
    public async Task<IActionResult> UpdateProviderCourseApprovedByRegulator(int ukprn, string larsCode, UpdateApprovedByRegulatorCommand command)
    {
        _logger.LogInformation("Outer API: Request to update confirm regulated standard for ukprn: {Ukprn} larscode: {Larscode}", ukprn, larsCode);
        command.Ukprn = ukprn;
        command.LarsCode = larsCode;
        try
        {
            await _mediator.Send(command);
        }
        catch (HttpRequestContentException ex)
        {
            _logger.LogError(ex, "Outer API: Failed request to update update confirm regulated standard for ukprn: {Ukprn} larscode: {Larscode} with HttpStatusCode: {HttpStatusCode}", ukprn, larsCode, ex.StatusCode);
            return new StatusCodeResult((int)ex.StatusCode);
        }
        return NoContent();
    }

    [HttpPost]
    [Route("providers/{ukprn}/courses/{larsCode}/update-online-delivery-option")]
    public async Task<IActionResult> UpdateOnlineDeliveryOption([FromRoute] int ukprn, [FromRoute] string larsCode, UpdateOnlineDeliveryOptionRequest request)
    {
        _logger.LogInformation("Outer API: Request to update online delivery option for ukprn: {Ukprn} larscode: {Larscode}", ukprn, larsCode);

        var command = new UpdateOnlineDeliveryOptionCommand()
        {
            Ukprn = ukprn,
            LarsCode = larsCode,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
            HasOnlineDeliveryOption = request.HasOnlineDeliveryOption
        };

        try
        {
            await _mediator.Send(command);
        }
        catch (HttpRequestContentException ex)
        {
            _logger.LogError(ex, "Outer API: Failed request to update online delivery option for ukprn: {Ukprn} larscode: {Larscode} with HttpStatusCode: {HttpStatusCode}", ukprn, larsCode, ex.StatusCode);
            return new StatusCodeResult((int)ex.StatusCode);
        }
        return NoContent();
    }
}
