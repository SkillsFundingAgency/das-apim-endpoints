﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<LocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPut]
        [Route ("{govUkIdentifier}/add-details")]
        public async Task <IActionResult> AddDetails ([FromRoute] string govUkIdentifier,[FromBody] CandidatesNameModel model)
        {
            try
            {
                var result = await _mediator.Send(new AddDetailsCommand
                {
                    FirstName = model.FirstName, 
                    LastName = model.LastName, 
                    GovUkIdentifier = govUkIdentifier, 
                    Email = model.Email
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error saving details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{govUkIdentifier}/date-of-birth")]
        public async Task<IActionResult> DateOfBirth([FromRoute] string govUkIdentifier, [FromBody] CandidatesDateOfBirthModel model)
        {
            try
            {
                var result = await _mediator.Send(new UpsertDateOfBirthCommand
                {
                    GovUkIdentifier = govUkIdentifier,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate date of birth details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
