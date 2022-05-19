﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProviderCourseController : ControllerBase
    {
        private readonly ILogger<ProviderCourseController> _logger;
        private readonly IMediator _mediator;

        public ProviderCourseController(ILogger<ProviderCourseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}/Course/{larsCode}/providerCourseLocation/{providerCourseId}")]
        public async Task<IActionResult> GetProviderCourse(int ukprn,int larsCode, int providerCourseId)
        {
            if (ukprn <= 9999999)
            {
                _logger.LogWarning("Invalid ukprn {ukprn}", ukprn);
                return BadRequest();
            }

            if (larsCode <= 0)
            {
                _logger.LogWarning("Invalid lars code {larsCode}", larsCode);
                return BadRequest();
            }

            if (providerCourseId <= 0)
            {
                _logger.LogWarning("Invalid provider CourseId {providerCourseId}", providerCourseId);
                return BadRequest();
            }

            var providerCourseResult = await _mediator.Send(new GetProviderCourseQuery(ukprn, larsCode, providerCourseId));

            if (providerCourseResult == null)
            {
                _logger.LogError($"Provider Course not found for ukprn {ukprn} and lars code {larsCode}");
                return NotFound();
            }

            return Ok(providerCourseResult);
        }
    }
}
