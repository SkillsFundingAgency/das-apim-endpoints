using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using System.Collections.Generic;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers
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
        public async Task<IActionResult> CreateEmployerRequest(CreateEmployerRequestCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result.EmployerRequestId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create employer request for RequestType: {command.RequestType}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{employerRequestId:guid}")]
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

        [HttpGet("provider/{ukprn}/active")]
        public async Task<IActionResult> GetAggregatedEmployerRequests(long ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

                var model = result.AggregatedEmployerRequests.Select(request => (AggregatedEmployerRequest)request).ToList();
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve aggregated employer requests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("provider/{ukprn}/selectrequests/{standardReference}")]
        public async Task<IActionResult> GetSelectEmployerRequests(string standardReference, long ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetSelectEmployerRequestsQuery() 
                {
                    StandardReference = standardReference,
                    Ukprn = ukprn
                });

                var model = (SelectEmployerRequests)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve select employer requests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("provider/{ukprn}/acknowledge-requests")]
        public async Task<IActionResult> AcknowledgeEmployerRequests(long ukprn, AcknowledgeRequestsParameters parameters)
        {
            try
            {
                await _mediator.Send(new AcknowledgeEmployerRequestsCommand
                { 
                    Ukprn = ukprn,
                    EmployerRequestIds = parameters.EmployerRequestIds,
                });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to save provider response for Employer Requests ");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("provider/{ukprn}/email-addresses")]
        public async Task<IActionResult> GetProviderEmailAddresses(long ukprn,[FromQuery]string userEmailAddress)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderEmailAddressesQuery()
                {
                    Ukprn = ukprn,
                    UserEmailAddress = userEmailAddress,
                });

                var model = (ProviderEmailAddresses)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve provider email addresses");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("provider/{ukprn}/phonenumbers")]
        public async Task<IActionResult> GetProviderPhoneNumbers(long ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderPhoneNumbersQuery()
                {
                    Ukprn = ukprn
                });

                var model = (ProviderPhoneNumbers)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve provider phone numbers");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("provider/{ukprn}/check-answers")]
        public async Task<IActionResult> GetCheckYourAnswers(long ukprn, [FromQuery]List<Guid> employerRequestIds)
        {
            try
            {
                var requestsResult = await _mediator.Send(new GetEmployerRequestsByIdsQuery()
                {
                    EmployerRequestIds = employerRequestIds,
                });

                var websiteResult = await _mediator.Send(new GetProviderWebsiteQuery()
                {
                    Ukprn = ukprn
                });

                var model = (CheckYourAnswers)requestsResult;
                model.Website = websiteResult.Website;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve Check Your Answers");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("provider/{ukprn}/submit-response")]
        public async Task<IActionResult> SubmitProviderResponse(long ukprn, SubmitProviderResponseParameters parameters)
        {
            try
            {
                var result = await _mediator.Send(new SubmitProviderResponseCommand 
                { 
                    Ukprn = ukprn,
                    CurrentUserEmail = parameters.CurrentUserEmail,
                    Email = parameters.Email,
                    EmployerRequestIds=parameters.EmployerRequestIds,
                    Phone = parameters.Phone,
                    Website = parameters.Website,
                });
                var model = (SubmitProviderResponse)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to save provider response for Employer Requests ");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("providerresponse/{providerResponseId:guid}/confirmation")]
        public async Task<IActionResult> GetProviderResponseConfirmation(Guid providerResponseId)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderResponseConfirmationQuery
                {
                    ProviderResponseId = providerResponseId
                });

                var model = (ProviderResponseConfirmation)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve provider response confirmation");
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
