using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Controllers.Application.Queries.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Controllers.Models;

namespace SFA.DAS.SharedOuterApi.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery]string email)
        {
            try
            {
                var result = await _mediator.Send(new GetUserAccountsQuery
                {
                    UserId = userId,
                    Email = email
                });

                return Ok((UserAccountsApiResponse) result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}