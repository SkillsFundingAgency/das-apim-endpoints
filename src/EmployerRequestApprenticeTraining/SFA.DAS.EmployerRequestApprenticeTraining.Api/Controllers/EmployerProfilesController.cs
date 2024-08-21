using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerProfilesController> _logger;

        public EmployerProfilesController(IMediator mediator, ILogger<EmployerProfilesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery]Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerProfileUserQuery { UserId = userId });
                return Ok((EmployerProfilesUser)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve user profile for {UserId}", userId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}