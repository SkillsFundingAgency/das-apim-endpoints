using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/leps-data/")]
    public class LepsDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LepsDataController> _logger;

        public LepsDataController(IMediator mediator, ILogger<LepsDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("")]
        public async Task<IActionResult> GetLepsDataWithUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetLEPSDataWithUsersQuery { });

                return Ok((GetLEPSDataListWithUsersResponse)result);
            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error getting leps data with users ");

                return BadRequest($"Error getting leps data with users. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}");
            }
        }
    }
}
