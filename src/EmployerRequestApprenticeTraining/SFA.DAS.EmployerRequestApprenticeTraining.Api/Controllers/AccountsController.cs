using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Extensions;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("accounts/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{accountId}/standard/{standardReference}/employer-request/existing")]
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

        [HttpPost("{accountId}/employer-requests")]
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
                    RequestedByFirstName = employerProfileUserResult.FirstName,
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

        [HttpGet("{accountId}/dashboard")]
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
    }
}
