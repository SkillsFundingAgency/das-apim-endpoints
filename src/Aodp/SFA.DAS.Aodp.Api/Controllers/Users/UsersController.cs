using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Models;


namespace SFA.DAS.AODP.Api.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger) : base(mediator, logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("reviewers")]
        public async Task<IActionResult> GetReviewers()
        {
            //Will be replaced with real reviewers from dfe sign-in Award-1040
            var reviewers = new List<User>
            {
                new() { Id = "Ada Lovelace", DisplayName = "Ada Lovelace", EmailAddress = "ada@lovelace.com" },
                new() { Id = "Alan Turing", DisplayName = "Alan Turing", EmailAddress = "alan@turing.com" },
                new() { Id = "Grace Hopper", DisplayName = "Grace Hopper" , EmailAddress = "grace@hopper.com"},
                new() { Id = "Katherine Johnson", DisplayName = "Katherine Johnson" , EmailAddress = "katherine@johnson.com"}
            };

            return Ok(new { users = reviewers });

        }

    }
}


