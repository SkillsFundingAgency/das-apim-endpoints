using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
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
                var locationResult = await _mediator.Send(new GetLocationQuery { ExactMatch = submitCommand.SingleLocation });
                if (locationResult.Location != null)
                {
                    var createCommand = new CreateEmployerRequestCommand
                    {
                        OriginalLocation = submitCommand.OriginalLocation,
                        RequestType = submitCommand.RequestType,
                        AccountId = submitCommand.AccountId,
                        StandardReference = submitCommand.StandardReference,
                        NumberOfApprentices = submitCommand.NumberOfApprentices,
                        SingleLocation = submitCommand.SingleLocation,
                        SingleLocationLatitude = locationResult.Location.Location.GeoPoint[0],
                        SingleLocationLongitude = locationResult.Location.Location.GeoPoint[1],
                        AtApprenticesWorkplace = submitCommand.AtApprenticesWorkplace,
                        DayRelease = submitCommand.DayRelease,
                        BlockRelease = submitCommand.BlockRelease,
                        RequestedBy = submitCommand.RequestedBy,
                        ModifiedBy = submitCommand.ModifiedBy
                    };

                    var result = await _mediator.Send(createCommand);
                    return Ok(result.EmployerRequestId);
                }

                return BadRequest($"Unable to submit employer request as the specified location {submitCommand.SingleLocation} cannot be found");
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

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetEmployerRequests(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestsQuery { AccountId = accountId });

                if (result.EmployerRequests != null)
                {
                    return Ok(result.EmployerRequests);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for AccoundId: {accountId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
