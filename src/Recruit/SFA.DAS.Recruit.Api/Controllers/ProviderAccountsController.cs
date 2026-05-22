using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;
using SFA.DAS.Recruit.Application.Queries.ProviderAccounts;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{ukprn}")]
    public class ProviderAccountsController(IMediator mediator) : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProviderStatus([FromRoute]int ukprn)
        {
            try
            {
                var result = await mediator.Send(new GetRoatpV2ProviderQuery
                {
                    Ukprn = ukprn
                });

                return Ok(new ProviderAccountResponse{CanAccessService = result});
            
            }
            catch (Exception)
            {
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("permissions")]
        public async Task<IActionResult> GetProviderPermissions([FromRoute] int ukprn)
        {
            try
            {
                var result = await mediator.Send(new GetProviderPermissionsByUkprnQuery(ukprn));

                return Ok(new GetProviderPermissionsResponse(result.Permissions));

            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}