using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetProvider;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("employer-requests/")]
    public class EmployerRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestsController> _logger;

        public EmployerRequestsController(IMediator mediator, ILogger<EmployerRequestsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{employerRequestId}/training-request")]
        public async Task<IActionResult> GetTrainingRequest([FromRoute] Guid employerRequestId, [FromQuery] bool includeProviders)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });

                if (employerRequestResult.EmployerRequest != null)
                {
                    var employerRequest = employerRequestResult.EmployerRequest;

                    List<Task<ProviderResponse>> providerTasks = new List<Task<ProviderResponse>>();
                    if (includeProviders)
                    {
                        providerTasks.AddRange(employerRequest.ProviderResponses.Select(async providerResponse =>
                        {
                            var providerResult = await _mediator.Send(new GetProviderQuery { Ukprn = providerResponse.Ukprn });
                            if (providerResult.Provider != null)
                            {
                                providerResponse.ProviderName = providerResult.Provider.Name;
                            }
                            return providerResponse;
                        }).ToList());
                    }

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardReference = employerRequest.StandardReference });
                    var settingsTask = _mediator.Send(new GetSettingsQuery());

                    var allTasks = providerTasks.Concat(new Task[] { standardTask, settingsTask }).ToList();
                    await Task.WhenAll(allTasks);

                    var providerResponses = await Task.WhenAll(providerTasks);
                    var standardResult = await standardTask;
                    var settings = await settingsTask;

                    var trainingRequest = new TrainingRequest
                    {
                        EmployerRequestId = employerRequest.Id,
                        StandardTitle = standardResult.Standard.StandardTitle,
                        StandardLevel = standardResult.Standard.StandardLevel,
                        NumberOfApprentices = employerRequest.NumberOfApprentices,
                        SameLocation = employerRequest.SameLocation,
                        SingleLocation = employerRequest.SingleLocation,
                        AtApprenticesWorkplace = employerRequest.AtApprenticesWorkplace,
                        DayRelease = employerRequest.DayRelease,
                        BlockRelease = employerRequest.BlockRelease,
                        RequestedAt = employerRequest.RequestedAt,
                        Status = employerRequest.RequestStatus,
                        ExpiredAt = employerRequest.ExpiredAt,
                        ExpiryAt = employerRequest.RequestedAt.AddMonths(settings.ExpiryAfterMonths),
                        RemoveAt = employerRequest.RequestedAt.AddMonths(settings.ExpiryAfterMonths + settings.RemovedAfterExpiryMonths),
                        Regions = employerRequest.Regions,
                        ProviderResponses = includeProviders ? providerResponses.ToList() : []
                    };

                    return Ok(trainingRequest);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve training request {EmployerRequestId}", employerRequestId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpPut("{employerRequestId}/responses/acknowledge")]
        public async Task<IActionResult> AcknowledgeProviderResponses([FromRoute] Guid employerRequestId, [FromQuery] Guid acknowledgedBy)
        {
            try
            {
                var command = new AcknowledgeProviderResponsesCommand
                {
                    EmployerRequestId = employerRequestId,
                    AcknowledgedBy = acknowledgedBy
                };

                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to acknowledge provider responses for {EmployerRequestId}", employerRequestId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{employerRequestId}/cancel")]
        public async Task<IActionResult> CancelEmployerRequest([FromRoute] Guid employerRequestId, [FromBody] CancelEmployerRequestRequest cancelRequest)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });
                if (employerRequestResult.EmployerRequest != null && employerRequestResult.EmployerRequest.RequestStatus == RequestStatus.Active)
                {
                    var standardTask = _mediator.Send(new GetStandardQuery { StandardReference = employerRequestResult.EmployerRequest.StandardReference });
                    var employerProfileTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = cancelRequest.CancelledBy });

                    await Task.WhenAll([standardTask, employerProfileTask]);

                    var standardResult = await standardTask;
                    var employerProfileResult = await employerProfileTask;

                    var command = new CancelEmployerRequestCommand
                    {
                        EmployerRequestId = employerRequestId,
                        CancelledBy = cancelRequest.CancelledBy,
                        CancelledByEmail = employerProfileResult.Email,
                        CancelledByFirstName = employerProfileResult.FirstName,
                        CourseLevel = $"{standardResult.Standard.StandardTitle} (level {standardResult.Standard.StandardLevel})",
                        DashboardUrl = cancelRequest.DashboardUrl
                    };

                    await _mediator.Send(command);
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to cancel employer request for {EmployerRequestId}", employerRequestId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{employerRequestId}/submit-confirmation")]
        public async Task<IActionResult> GetSubmitEmployerRequestConfirmation([FromRoute] Guid employerRequestId)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });

                if (employerRequestResult.EmployerRequest != null)
                {
                    var employerRequest = employerRequestResult.EmployerRequest;

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardReference = employerRequest.StandardReference });
                    var employerProfileUserTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = employerRequest.RequestedBy });
                    var settingsTask = _mediator.Send(new GetSettingsQuery());

                    await Task.WhenAll(standardTask, employerProfileUserTask, settingsTask);

                    var standardResult = await standardTask;
                    var employerProfileUser = await employerProfileUserTask;
                    var settings = await settingsTask;

                    return Ok(new SubmitEmployerRequestConfirmation
                    {
                        EmployerRequestId = employerRequest.Id,
                        StandardTitle = standardResult.Standard.StandardTitle,
                        StandardLevel = standardResult.Standard.StandardLevel,
                        NumberOfApprentices = employerRequest.NumberOfApprentices,
                        SameLocation = employerRequest.SameLocation,
                        SingleLocation = employerRequest.SingleLocation,
                        AtApprenticesWorkplace = employerRequest.AtApprenticesWorkplace,
                        DayRelease = employerRequest.DayRelease,
                        BlockRelease = employerRequest.BlockRelease,
                        RequestedByEmail = employerProfileUser.Email,
                        ExpiryAfterMonths = settings.ExpiryAfterMonths,
                        Regions = employerRequest.Regions
                    });
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve submit employer request confirmation for {EmployerRequestId}", employerRequestId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{employerRequestId}/cancel-confirmation")]
        public async Task<IActionResult> GetCancelEmployerRequestConfirmation([FromRoute] Guid employerRequestId)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });

                if (employerRequestResult.EmployerRequest != null)
                {
                    var employerRequest = employerRequestResult.EmployerRequest;

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardReference = employerRequest.StandardReference });
                    var employerProfileUserTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = employerRequest.RequestedBy });

                    await Task.WhenAll(standardTask, employerProfileUserTask);

                    var standardResult = await standardTask;
                    var employerProfileUser = await employerProfileUserTask;

                    return Ok(new CancelEmployerRequestConfirmation
                    {
                        EmployerRequestId = employerRequest.Id,
                        StandardTitle = standardResult.Standard.StandardTitle,
                        StandardLevel = standardResult.Standard.StandardLevel,
                        NumberOfApprentices = employerRequest.NumberOfApprentices,
                        SameLocation = employerRequest.SameLocation,
                        SingleLocation = employerRequest.SingleLocation,
                        AtApprenticesWorkplace = employerRequest.AtApprenticesWorkplace,
                        DayRelease = employerRequest.DayRelease,
                        BlockRelease = employerRequest.BlockRelease,
                        CancelledByEmail = employerProfileUser.Email,
                        Regions = employerRequest.Regions
                    });
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve cancel employer request confirmation for {EmployerRequestId}", employerRequestId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("expire")]
        public async Task<IActionResult> ExpireEmployerRequests()
        {
            try
            {
                await _mediator.Send(new ExpireEmployerRequestsCommand());
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to expire employer requests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("response-notifications")]
        public async Task<IActionResult> GetEmployerRequestsForResponseNotification()
        {
            try
            {
                _logger.LogInformation("GetEmployerRequestsForResponseNotification call initiated");
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestsForResponseNotificationQuery());
                return Ok(employerRequestResult.EmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve employer requests for response notification");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("send-notification")]
        public async Task<IActionResult> SendEmployerRequestResponseNotifications(SendResponseNotificationEmailParameters parameters)
        {
            try
            {
                var employerProfileUserResult = await _mediator.Send(new GetEmployerProfileUserQuery { UserId = parameters.RequestedBy });

                bool canUserReceiveNotifications = await _mediator.Send(new CanUserReceiveNotificationsQuery { UserId = parameters.RequestedBy, AccountId = parameters.AccountId });

                if (canUserReceiveNotifications)
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
                            })
                            .OrderBy(s => s.StandardTitle)
                            .ToList(),
                        ManageRequestsLink = parameters.ManageRequestsLink,
                        ManageNotificationSettingsLink = parameters.ManageNotificationSettingsLink,
                    });
                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to send employer request response notification for {AccountId}", parameters.AccountId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
