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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.ManuallyEnteredAddress;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetDateOfBirth;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CandidatePreferences;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CheckAnswers;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.PhoneNumber;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.CheckAnswers;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.PhoneNumber;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;

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
        [Route("{candidateId}/create-account/add-details")]
        public async Task<IActionResult> AddDetails([FromRoute] Guid candidateId, [FromBody] CandidatesNameModel model)
        {
            try
            {
                var result = await _mediator.Send(new AddDetailsCommand
                {
                    CandidateId = candidateId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
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

        [HttpGet]
        [Route("{candidateId}/create-account/user-name")]
        public async Task<IActionResult> UserName([FromRoute] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetCandidateNameQuery
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error getting user name");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{candidateId}/create-account/date-of-birth")]
        public async Task<IActionResult> DateOfBirth([FromRoute] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetDateOfBirthQuery
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting candidate date of birth details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{candidateId}/create-account/date-of-birth")]
        public async Task<IActionResult> DateOfBirth([FromRoute] Guid candidateId, [FromBody] CandidatesDateOfBirthModel model)
        {
            try
            {
                var result = await _mediator.Send(new UpsertDateOfBirthCommand
                {
                    CandidateId = candidateId,
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
        [Route("create-account/postcode-address")]
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
        [Route("{candidateId}/create-account/select-address")]
        public async Task<IActionResult> SelectAddress([FromRoute] Guid candidateId, [FromQuery] string postcode)
        {
            try
            {
                var result = await _mediator.Send(new GetCandidateAddressesByPostcodeQuery
                {
                    CandidateId = candidateId,
                    Postcode = postcode
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting addresses by postcode");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{candidateId}/create-account/user-address")]
        public async Task<IActionResult> UserAddress([FromRoute] Guid candidateId)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetCandidateAddressQuery
                {
                    CandidateId = candidateId
                });

                return Ok(queryResponse);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Error getting user address");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{candidateId}/create-account/select-address")]
        public async Task<IActionResult> SelectAddress([FromRoute] Guid candidateId, [FromBody] CandidatesAddressModel model)
        {
            try
            {
                var result = await _mediator.Send(new CreateAddressCommand
                {
                    CandidateId = candidateId,
                    Uprn = model.Uprn,
                    Email = model.Email,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    AddressLine3 = model.AddressLine3,
                    AddressLine4 = model.AddressLine4,
                    Postcode = model.Postcode
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate address details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{candidateId}/create-account/enter-address")]
        public async Task<IActionResult> EnterAddress([FromRoute] Guid candidateId, [FromBody] CandidatesManuallyEnteredAddressModel model)
        {
            try
            {
                var result = await _mediator.Send(new CreateManuallyEnteredAddressCommand
                {
                    CandidateId = candidateId,
                    Email = model.Email,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    TownOrCity = model.TownOrCity,
                    County = model.County,
                    Postcode = model.Postcode
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate manually entered address details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{candidateId}/create-account/phone-number")]
        public async Task<IActionResult> PhoneNumber([FromRoute] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetPhoneNumberQuery
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting phone number");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{candidateId}/create-account/phone-number")]
        public async Task<IActionResult> PhoneNumber([FromRoute] Guid candidateId, [FromBody] CandidatesPhoneNumberModel model)
        {
            try
            {
                var result = await _mediator.Send(new CreatePhoneNumberCommand
                {
                    CandidateId = candidateId,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate phone number");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{candidateId}/create-account/candidate-preferences")]
        public async Task<IActionResult> GetCandidatePreferences([FromRoute] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetCandidatePreferencesQuery()
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting candidate preferences");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{candidateId}/create-account/candidate-preferences")]
        public async Task<IActionResult> UpsertCandidatePreferences([FromRoute] Guid candidateId, [FromBody] CandidatePreferencesModel model)
        {
            try
            {
                var result = await _mediator.Send(new UpsertCandidatePreferencesCommand
                {
                    CandidateId = candidateId,
                    CandidatePreferences = model.CandidatePreferences.Select(x => new UpsertCandidatePreferencesCommand.CandidatePreference
                    {
                        PreferenceId = x.PreferenceId,
                        PreferenceMeaning = x.PreferenceMeaning,
                        PreferenceHint = x.PreferenceHint,
                        ContactMethodsAndStatus = x.ContactMethodsAndStatus.Select(x => new UpsertCandidatePreferencesCommand.ContactMethodStatus
                        {
                            ContactMethod = x.ContactMethod,
                            Status = x.Status
                        }).ToList()
                    }).ToList()
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error upserting candidate preferences");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{candidateId}/create-account/check-answers")]
        public async Task<IActionResult> GetCheckAnswers([FromRoute] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetCheckAnswersQuery
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting candidate create account check answers");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{candidateId}/create-account/check-answers")]
        public async Task<IActionResult> PostCheckAnswers([FromRoute] Guid candidateId)
        {
            try
            {
                await _mediator.Send(new UpdateCheckAnswersCommand
                {
                    CandidateId = candidateId
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error posting candidate create account check answers");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
