using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("providers/")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{ukprn}/active")]
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

        [HttpGet("{ukprn}/employer-requests/{standardReference}/select")]
        public async Task<IActionResult> GetSelectEmployerRequests(string standardReference, long ukprn)
        {
            try
            {
                var selectEmployerRequestsTask = _mediator.Send(new GetSelectEmployerRequestsQuery() 
                {
                    StandardReference = standardReference,
                    Ukprn = ukprn
                });

                var settingsTask = _mediator.Send(new GetSettingsQuery());

                await Task.WhenAll(selectEmployerRequestsTask, settingsTask);

                var selectEmployerRequestsResult = await selectEmployerRequestsTask;
                var settings = await settingsTask;

                var model = (SelectEmployerRequests)selectEmployerRequestsResult;
                model.ExpiryAfterMonths = settings.ExpiryAfterMonths;
                model.RemovedAfterRequestedMonths = settings.RemovedAfterRequestedMonths;
                
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve select employer requests");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{ukprn}/employer-requests/acknowledge")]
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

        [HttpGet("{ukprn}/email-addresses")]
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

        [HttpGet("{ukprn}/phone-numbers")]
        public async Task<IActionResult> GetProviderPhoneNumbers(long ukprn)
        {
            try
            {
                _logger.LogInformation("GetProviderPhoneNumbers call initiated");
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

        [HttpGet("{ukprn}/check-answers")]
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

        [HttpPost("{ukprn}/responses")]
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
                    RespondedBy = parameters.RespondedBy,
                    ContactName = parameters.ContactName,
                    CurrentUserFirstName = parameters.CurrentUserFirstName,
                    StandardLevel = parameters.StandardLevel,
                    StandardTitle = parameters.StandardTitle,
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
    }
}
