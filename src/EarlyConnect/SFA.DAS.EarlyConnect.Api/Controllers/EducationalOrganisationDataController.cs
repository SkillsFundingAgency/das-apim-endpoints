using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode;
using System.Net;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Api.Requests.GetRequests;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/educational-organisations-data/")]
    public class EducationalOrganisationDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EducationalOrganisationDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEducationalOrganisationsData([FromQuery] EducationalOrganisationsGetRequest educationalOrganisationsGetRequest)
        {
            var result = await _mediator.Send(new GetEducationalOrganisationsByLepCodeQuery
            {
                LepCode = educationalOrganisationsGetRequest.LepCode,
                SearchTerm = educationalOrganisationsGetRequest.SearchTerm,
                PageSize = educationalOrganisationsGetRequest.PageSize,
                Page = educationalOrganisationsGetRequest.Page
            });

       

            return Ok((GetEducationalOrganisationsResponse)result);
        }

    }
}
