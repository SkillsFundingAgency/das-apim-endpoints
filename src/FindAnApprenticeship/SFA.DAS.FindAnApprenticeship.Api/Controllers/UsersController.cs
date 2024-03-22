using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Address;

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
        [Route("{govUkIdentifier}/add-details")]
        public async Task<IActionResult> AddDetails([FromRoute] string govUkIdentifier, [FromBody] CandidatesNameModel model)
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

        [HttpGet]
        [Route("postcode-address")]
        public async Task<IActionResult> PostcodeAddress([FromQuery] string postcode)
        {
            try
            {
                var result = await _mediator.Send(new GetCandidatePostcodeAddressQuery { Postcode = postcode });
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting candidate PostcodeAddress");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("select-address")]
        public async Task<IActionResult> SelectAddress([FromQuery] string postcode)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetCandidateAddressesByPostcodeQuery(postcode));

                if (queryResponse.AddressesResponse == null || !queryResponse.AddressesResponse.Addresses.Any())
                    return NotFound();

                return Ok(queryResponse.AddressesResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting addresses by postcode");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{govUkIdentifier}/select-address")]
        public async Task<IActionResult> SelectAddress([FromRoute] string govUkIdentifier, [FromBody] CandidatesAddressModel model)
        {
            try
            {
                var result = await _mediator.Send(new CreateAddressCommand
                {
                    GovUkIdentifier = govUkIdentifier,
                    Email = model.Email,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    AddressLine3 = model.AddressLine3,
                    AddressLine4 = model.AddressLine4,
                    Postcode = model.Postcode,
                    Uprn = model.Uprn
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate address details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
