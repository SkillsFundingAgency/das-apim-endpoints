using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.ProviderAccounts.Queries;
using SFA.DAS.Approvals.Application.Providers.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProviderAccountsController : Controller
    {
        private readonly IMediator _mediator;

        public ProviderAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProviderStatus([FromRoute]int ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetRoatpV2ProviderQuery
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
    }
}