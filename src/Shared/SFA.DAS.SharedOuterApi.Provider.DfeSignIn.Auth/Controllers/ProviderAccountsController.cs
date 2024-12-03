using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProviderAccountsController> _logger;

        public ProviderAccountsController(IMediator mediator, ILogger<ProviderAccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProviderStatus([FromRoute] int ukprn)
        {
            try
            {
                //var result = await _mediator.Send(new GetRoatpV2ProviderQuery
                //{
                //    Ukprn = ukprn
                //});

                return Ok(new ProviderAccountResponse { CanAccessService = true });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get provider status for ukprn {ukprn}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}