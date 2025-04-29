using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Learners.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class SelectLearnersController(IMediator mediator, ILogger<SelectLearnersController> logger) : Controller
    {
        [HttpGet]
        [Route("/providers/{providerId}/unapproved/add/learners/select")]
        public async Task<IActionResult> Get(
            long providerId,
            [FromQuery] long accountLegalEntityId,
            [FromQuery] string searchTerm,
            [FromQuery] string sortColumn,
            [FromQuery] bool sortDescending,
            [FromQuery] int page,
            [FromQuery] int? pageSize)
        {
            try
            {
                logger.LogInformation("Getting ILR records for Provider {providerId}", providerId);
                var result = await mediator.Send(new GetLearnersForProviderQuery
                {
                    ProviderId = providerId, 
                    AccountLegalEntityId = accountLegalEntityId, 
                    SearchTerm = searchTerm,
                    SortField = sortColumn, 
                    SortDescending = sortDescending, 
                    Page = page,
                    PageSize = pageSize
                });
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error when getting  ILR records for Provider {providerId}", providerId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}