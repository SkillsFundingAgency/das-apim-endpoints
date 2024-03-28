using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Application.Queries.ProviderAccounts;
using SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Models;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Controllers
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
        public async Task<IActionResult> GetProviderStatus([FromRoute] int ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetRoatpV2ProviderQuery
                {
                    Ukprn = ukprn
                });

                return Ok(new ProviderAccountResponse { CanAccessService = result });

            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}