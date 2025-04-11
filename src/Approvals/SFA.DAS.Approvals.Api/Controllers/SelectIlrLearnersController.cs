using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SFA.DAS.Approvals.Application.Learners.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class SelectIlrLearnersController(IMediator mediator, ILogger<SelectIlrLearnersController> logger) : Controller
    {
        [HttpGet]
        [Route("/providers/{providerId}/unapproved/add/ilrs/select")]
        public async Task<IActionResult> Get(
            long providerId,
            [FromQuery] long accountLegalEntityId,
            [FromQuery] string searchTerm,
            [FromQuery] string sortColumn,
            [FromQuery] bool sortDescending,
            [FromQuery] int page)
        {
            try
            {
                logger.LogInformation("Getting ILR records for Provider {providerId}", providerId);
                var result = await mediator.Send(new GetLearnersForProviderQuery {  });
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