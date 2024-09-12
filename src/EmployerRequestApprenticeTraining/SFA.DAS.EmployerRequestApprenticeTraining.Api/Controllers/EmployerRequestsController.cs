using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Extensions;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
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
    [Route("[controller]/")]
    public class EmployerRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestsController> _logger;

        public EmployerRequestsController(IMediator mediator, ILogger<EmployerRequestsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("account/{accountId}/standard/{standardReference}/existing")]
        public async Task<IActionResult> HasExistingEmployerRequest([FromRoute] long accountId, [FromRoute] string standardReference)
        {
            try
            {
                var result = await _mediator.Send(new GetActiveEmployerRequestQuery { AccountId = accountId, StandardReference = standardReference });
                return Ok(result.EmployerRequest != null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error checking for existing employer request for {AccountId} and {StandardReference}", accountId, standardReference.SanitizeLogData());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("account/{accountId}/submit-request")]
        public async Task<IActionResult> SubmitEmployerRequest([FromRoute] long accountId, [FromBody] SubmitEmployerRequestRequest submitRequest)
        {
            try
            {
                var employerProfileUserResult = await _mediator.Send(new GetEmployerProfileUserQuery { UserId = submitRequest.RequestedBy });

                GetLocationResult locationResult = null;
                if (string.IsNullOrEmpty(submitRequest.SameLocation) || submitRequest.SameLocation == "Yes")
                {
                    locationResult = await _mediator.Send(new GetLocationQuery { ExactSearchTerm = submitRequest.SingleLocation });
                    if (locationResult.Location == null)
                    {
                        return BadRequest($"Unable to submit employer request as the specified location {submitRequest.SingleLocation} cannot be found");
                    }
                }

                var standardTask = _mediator.Send(new GetStandardQuery { StandardId = submitRequest.StandardReference });
                var settingsTask = _mediator.Send(new GetSettingsQuery());

                await Task.WhenAll([standardTask, settingsTask]);

                var standardResult = await standardTask;
                var settings = await settingsTask;

                var createCommand = new SubmitEmployerRequestCommand
                {
                    OriginalLocation = submitRequest.OriginalLocation,
                    RequestType = submitRequest.RequestType,
                    AccountId = accountId,
                    StandardReference = submitRequest.StandardReference,
                    NumberOfApprentices = submitRequest.NumberOfApprentices,
                    SameLocation = submitRequest.SameLocation,
                    SingleLocation = submitRequest.SingleLocation,
                    SingleLocationLatitude = locationResult?.Location.Location.GeoPoint[0] ?? 0.0,
                    SingleLocationLongitude = locationResult?.Location.Location.GeoPoint[1] ?? 0.0,
                    MultipleLocations = submitRequest.MultipleLocations,
                    AtApprenticesWorkplace = submitRequest.AtApprenticesWorkplace,
                    DayRelease = submitRequest.DayRelease,
                    BlockRelease = submitRequest.BlockRelease,
                    RequestedBy = submitRequest.RequestedBy,
                    RequestedByEmail = employerProfileUserResult.Email,
                    ModifiedBy = submitRequest.ModifiedBy,
                    CourseLevel = $"{standardResult.Standard.Title} (level {standardResult.Standard.Level})",
                    ExpiryAfterMonths = settings.ExpiryAfterMonths,
                    DashboardUrl = submitRequest.DashboardUrl
                };

                var result = await _mediator.Send(createCommand);
                return Ok(result.EmployerRequestId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to submit employer request for {RequestType}", submitRequest.RequestType);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("account/{accountId}/dashboard")]
        public async Task<IActionResult> GetDashboard([FromRoute] long accountId)
        {
            try
            {
                var aggregatedEmployerRequestsTask = _mediator.Send(new GetAggregatedEmployerRequestsQuery(accountId));
                var settingsTask = _mediator.Send(new GetSettingsQuery());

                await Task.WhenAll(aggregatedEmployerRequestsTask, settingsTask);

                var aggregatedEmployerRequestsResults = await aggregatedEmployerRequestsTask;
                var settingsResult = await settingsTask;

                var result = new Dashboard
                {
                    AggregatedEmployerRequests = aggregatedEmployerRequestsResults.AggregatedEmployerRequests.Select(request => (AggregatedEmployerRequest)request).ToList(),
                    ExpiryAfterMonths = settingsResult.ExpiryAfterMonths,
                    RemovedAfterExpiryMonths = settingsResult.RemovedAfterExpiryMonths
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve dashboard");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
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

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardId = employerRequest.StandardReference });
                    var settingsTask = _mediator.Send(new GetSettingsQuery());

                    var allTasks = providerTasks.Concat(new Task[] { standardTask, settingsTask }).ToList();
                    await Task.WhenAll(allTasks);

                    var providerResponses = await Task.WhenAll(providerTasks); 
                    var standardResult = await standardTask;
                    var settings = await settingsTask;

                    var trainingRequest = new TrainingRequest
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
                        RequestedAt = employerRequest.RequestedAt,
                        Status = employerRequest.Status,
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


        [HttpPut("{employerRequestId}/acknowledge-responses")]
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

        [HttpPut("{employerRequestId}/cancel-request")]
        public async Task<IActionResult> CancelEmployerRequest([FromRoute] Guid employerRequestId, [FromBody] CancelEmployerRequestRequest cancelRequest)
        {
            try
            {
                var employerRequestResult = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });
                if (employerRequestResult.EmployerRequest != null && employerRequestResult.EmployerRequest.Status == RequestStatus.Active)
                {
                    var standardTask = _mediator.Send(new GetStandardQuery { StandardId = employerRequestResult.EmployerRequest.StandardReference });
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
                        CourseLevel = $"{standardResult.Standard.Title} (level {standardResult.Standard.Level})",
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

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardId = employerRequest.StandardReference });
                    var employerProfileUserTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = employerRequest.RequestedBy });
                    var settingsTask = _mediator.Send(new GetSettingsQuery());

                    await Task.WhenAll(standardTask, employerProfileUserTask, settingsTask);

                    var standardResult = await standardTask;
                    var employerProfileUser = await employerProfileUserTask;
                    var settings = await settingsTask;

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

                    var standardTask = _mediator.Send(new GetStandardQuery { StandardId = employerRequest.StandardReference });
                    var employerProfileUserTask = _mediator.Send(new GetEmployerProfileUserQuery { UserId = employerRequest.RequestedBy });

                    await Task.WhenAll(standardTask, employerProfileUserTask);

                    var standardResult = await standardTask;
                    var employerProfileUser = await employerProfileUserTask;

                    return Ok(new CancelEmployerRequestConfirmation
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

        [HttpPost("expire-requests")]
        public async Task<IActionResult> ExpireEmployerRequests()
        {
            try
            {
                await _mediator.Send(new ExpireEmployerRequestsCommand());
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to expire employer requests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
