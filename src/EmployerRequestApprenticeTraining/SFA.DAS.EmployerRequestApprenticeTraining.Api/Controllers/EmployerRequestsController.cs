using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Extensions;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerRequestsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestsController> _logger;

        public EmployerRequestsController(IMediator mediator, ILogger<EmployerRequestsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEmployerRequest(SubmitEmployerRequestCommand submitCommand)
        {
            try
            {
                var employerProfileUserResult = await _mediator.Send(new GetEmployerProfileUserQuery { UserId = submitCommand.RequestedBy });

                GetLocationResult locationResult = null;
                if (string.IsNullOrEmpty(submitCommand.SameLocation) || submitCommand.SameLocation == "Yes")
                {
                    locationResult = await _mediator.Send(new GetLocationQuery { ExactSearchTerm = submitCommand.SingleLocation });
                    if (locationResult.Location == null)
                    {
                        return BadRequest($"Unable to submit employer request as the specified location {submitCommand.SingleLocation} cannot be found");
                    }
                }

                var createCommand = new CreateEmployerRequestCommand
                {
                    OriginalLocation = submitCommand.OriginalLocation,
                    RequestType = submitCommand.RequestType,
                    AccountId = submitCommand.AccountId,
                    StandardReference = submitCommand.StandardReference,
                    NumberOfApprentices = submitCommand.NumberOfApprentices,
                    SameLocation = submitCommand.SameLocation,
                    SingleLocation = submitCommand.SingleLocation,
                    SingleLocationLatitude = locationResult?.Location.Location.GeoPoint[0] ?? 0.0,
                    SingleLocationLongitude = locationResult?.Location.Location.GeoPoint[1] ?? 0.0,
                    MultipleLocations = submitCommand.MultipleLocations,
                    AtApprenticesWorkplace = submitCommand.AtApprenticesWorkplace,
                    DayRelease = submitCommand.DayRelease,
                    BlockRelease = submitCommand.BlockRelease,
                    RequestedBy = submitCommand.RequestedBy,
                    RequestedByEmail = employerProfileUserResult.Email,
                    ModifiedBy = submitCommand.ModifiedBy
                };

                var result = await _mediator.Send(createCommand);
                return Ok(result.EmployerRequestId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to submit employer request for RequestType: {submitCommand.RequestType}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{employerRequestId}")]
        public async Task<IActionResult> GetEmployerRequest(Guid employerRequestId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });

                if (result.EmployerRequest != null)
                {
                    return Ok(result.EmployerRequest);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer request for EmployerRequestId: {employerRequestId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("account/{accountId}/standard/{standardReference}")]
        public async Task<IActionResult> GetEmployerRequest(long accountId, string standardReference)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { AccountId = accountId, StandardReference = standardReference });
                return Ok(result.EmployerRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer request for AccountId: {accountId} and standardReference: {standardReference.SanitizeLogData()}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetEmployerRequests(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestsQuery { AccountId = accountId });
                return Ok(result.EmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for AccoundId: {accountId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{employerRequestId}/submit-confirmation")]
        public async Task<IActionResult> GetSubmitEmployerRequestConfirmation(Guid employerRequestId)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });

                if (employerRequestResult.EmployerRequest != null)
                {
                    var employerRequest = employerRequestResult.EmployerRequest;

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardId = employerRequest.StandardReference });
                    var employerProfileUserTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = employerRequest.RequestedBy });

                    await Task.WhenAll(standardTask, employerProfileUserTask);

                    var standardResult = await standardTask;
                    var employerProfileUser = await employerProfileUserTask;

                    return Ok(new SubmitEmployerRequestConfirmation
                    {
                        EmployerRequestId = employerRequest.Id,
                        StandardTitle = standardResult.Standard.Title,
                        StandardLevel = standardResult.Standard.Level,
                        NumberOfApprentices = employerRequest.NumberOfApprentices,
                        SameLocation = employerRequest.SameLocation,
                        SingleLocation = employerRequest.SingleLocation,
                        AtApprenticesWorkplace = employerRequest.AtApprenticesWorkplace,
                        DayRelease = employerRequest.DayRelease,
                        BlockRelease = employerRequest.BlockRelease,
                        RequestedByEmail = employerProfileUser.Email,
                        Regions = employerRequest.Regions
                    });
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve submit employer request confirmation for EmployerRequestId: {employerRequestId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("requests-for-response-notification")]
        public async Task<IActionResult> GetEmployerRequestsForResponseNotification()
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestsForResponseNotificationQuery());
                return Ok(employerRequestResult.EmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for response notification");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("send-notifications")]
        public async Task<IActionResult> SendEmployerRequestResponseNotifications(SendResponseNotificationEmailParameters parameters)
        {
            try
            {
                var employerProfileUserResult = await _mediator.Send(new GetEmployerProfileUserQuery { UserId = parameters.RequestedBy });

                bool canUserReceiveNotifications = await _mediator.Send(new CanUserReceiveNotificationsQuery { UserId = parameters.RequestedBy, AccountId = parameters.AccountId });

                if(canUserReceiveNotifications) 
                {
                    await _mediator.Send(new SendResponseNotificationCommand()
                    {
                        EmailAddress = employerProfileUserResult.Email,
                        FirstName = employerProfileUserResult.FirstName,
                        RequestedBy = parameters.RequestedBy,
                        AccountId = parameters.AccountId,
                        Standards = parameters.Standards.Select(e =>
                        new Application.Commands.SendResponseNotification.StandardDetails
                        {
                            StandardLevel = e.StandardLevel,
                            StandardTitle = e.StandardTitle,
                        }).ToList(),
                    });
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to send employer request response notification for : {0}", parameters.AccountId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
