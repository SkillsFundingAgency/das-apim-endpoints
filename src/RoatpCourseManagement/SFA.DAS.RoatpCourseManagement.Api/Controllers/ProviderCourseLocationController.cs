﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseLocationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseLocationController> _logger;

        public ProviderCourseLocationController(ILogger<ProviderCourseLocationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/courses/{larsCode}/locations/provider-locations")]
        public async Task<IActionResult> GetProviderCourseLocations(int ukprn, int larsCode)
        {
            _logger.LogInformation("Outer API: Request received to get provider course locations for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
            var providerCourselocationsResult = await _mediator.Send(new GetProviderCourseLocationQuery(ukprn, larsCode));
            if (providerCourselocationsResult == null)
            {
                _logger.LogError($"Provider Course Locations not found for ukprn {ukprn} and lars code {larsCode}");
                return NotFound();
            }

            return Ok(providerCourselocationsResult);
        }
    }
}
