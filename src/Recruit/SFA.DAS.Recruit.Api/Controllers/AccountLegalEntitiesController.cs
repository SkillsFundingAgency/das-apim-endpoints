using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.Providers.Responses;
using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprn;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

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

        [HttpGet]
        [Route("provider/{ukprn:int}")]
        public async Task<IResult> GetProviderPermissionsByUkprn([FromRoute] int ukprn, [FromQuery] List<Operation> operations)
        {
            try
            {
                var result = await mediator.Send(new GetProviderPermissionsByUkprnQuery(ukprn, operations));
                return TypedResults.Ok(new GetProviderPermissionsApiResponse { AccountProviderLegalEntities = result.LegalEntities });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting provider permissions by UKPRN");
                return TypedResults.InternalServerError();
            }
        }

        [HttpGet]
        [Route("employerAccount/{accountHashedId}")]
        public async Task<IResult> GetEmployerPermissionsByAccountId([FromRoute] string accountHashedId, [FromQuery] List<Operation> operations)
        {
            try
            {
                var result = await mediator.Send(new GetEmployerAccountLegalEntitiesQuery(accountHashedId, operations));
                return TypedResults.Ok(new GetProviderPermissionsApiResponse { AccountProviderLegalEntities = result.LegalEntities });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer permissions by account hashed ID");
                return TypedResults.InternalServerError();
            }
        }

        [HttpGet]
        [Route("provider/{ukprn:int}/employerAccount/{accountId:long}")]
        public async Task<IResult> GetProviderPermissionsByUkprnAndEmployerAccountId([FromRoute] int ukprn, [FromRoute] long accountId, [FromQuery] List<Operation> operations)
        {
            try
            {
                var result = await mediator.Send(new GetProviderPermissionsByUkprnAndAccountIdQuery(ukprn, accountId, operations));
                return TypedResults.Ok(new GetProviderPermissionsApiResponse { AccountProviderLegalEntities = result.LegalEntities });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting provider permissions by UKPRN and Employer Account ID");
                return TypedResults.InternalServerError();
            }
        }
    }
}
