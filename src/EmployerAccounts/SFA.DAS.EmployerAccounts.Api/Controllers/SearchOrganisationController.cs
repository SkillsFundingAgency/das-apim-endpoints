using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCharity;
using SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using static SFA.DAS.EmployerAccounts.Api.Models.FindPublicSectorOrganisationResponse;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SearchOrganisationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SearchOrganisationController> _logger;

        public SearchOrganisationController(IMediator mediator, ILogger<SearchOrganisationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("", Name = "Search")]
        public async Task<IActionResult> SearchOrganisations([FromQuery] string searchTerm, [FromQuery] int maximumResults = 500)
        {
            try
            {
                var result = await _mediator.Send(new SearchOrganisationsQuery()
                {
                    SearchTerm = searchTerm,
                    MaximumResults = maximumResults
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (SearchOrganisationsResponse)result;

                return Ok(model.Organisations);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Searching for Organisations");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetLatestDetails([FromQuery] string identifier, [FromQuery] OrganisationType organisationType)
        {
            var result = await _mediator.Send(new GetLatestDetailsQuery()
            {
                Identifier = identifier,
                OrganisationType = organisationType
            });

            var model = (GetLatestDetailsResponse)result;

            return Ok(model.Organisation);
        }

        [HttpGet]
        [Route("charities/{registrationNumber}", Name = "Charity")]
        public async Task<IActionResult> GetCharity(int registrationNumber)
        {
            try
            {
                var result = await _mediator.Send(new GetCharityQuery()
                {
                    RegistrationNumber = registrationNumber
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetCharityResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Charity");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("publicsectorbodies", Name = "Public Sector")]
        public async Task<IActionResult> GetPublicSectorOrganisations([FromQuery] string searchTerm, [FromQuery] int pageSize = 1000, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var result = await _mediator.Send(new FindPublicSectorOrganisationQuery()
                {
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageNumber = pageNumber
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = new FindPublicSectorOrganisationResponse()
                {
                    Data = result.Data.Select(x => (PublicSectorOrganisation)x).ToList(),
                    PageNumber = result.Page,
                    TotalPages = result.TotalPages
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting PublicSectorOrganisations");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("IdentifiableOrganisationTypes")]
        public async Task<IActionResult> GetIdentifiableOrganisationTypes()
        {
            try
            {
                var result = await _mediator.Send(new GetIdentifiableOrganisationTypesQuery());

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetIdentifiableOrganisationTypesResponse)result;

                return Ok(model.OrganisationTypes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting IdentifiableOrganisationTypes");
                return BadRequest();
            }
        }
    }
}
