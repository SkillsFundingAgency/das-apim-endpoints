using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountLegalEntitiesController(IMediator mediator,
        ILogger<AccountLegalEntitiesController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("getAllLegalEntities")]
        public async Task<IActionResult> GetAllAccountLegalEntities(
            [FromBody] GetAllLegalEntitiesRequest request,
            CancellationToken token = default)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAllAccountLegalEntitiesQuery(request.SearchTerm,
                    request.AccountIds,
                    request.PageNumber,
                    request.PageSize,
                    request.SortColumn,
                    request.IsAscending), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting all legal entities for account");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
