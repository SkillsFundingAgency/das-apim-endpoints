
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class RoiController : ControllerBase
    {
        private readonly ILogger<RoiController> _logger;
        private readonly IMediator _mediator;

        public RoiController (ILogger<RoiController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ApprenticeshipCategories")]
        public async Task<IActionResult> GetApprenticeshipCategories()
        {
            try
            {
                var entries = new List<DictionaryEntry> {
                    new DictionaryEntry("cat1", "Business and Aministration"),
                    new DictionaryEntry("cat2", "Care Services"),
                    new DictionaryEntry("cat3", "Catering and Hospitality"),
                };
                return Ok(entries);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting apprenticeship categories");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("QualificationLevels")]
        public async Task<IActionResult> GetQualificationLevels()
        {
            try
            {
                var entries = new List<DictionaryEntry> {
                    new DictionaryEntry("ql2", "Level 2 - GCSE"),
                    new DictionaryEntry("ql3", "Level 3 - A Level"),
                    new DictionaryEntry("ql4", "Level 4 - Higher National Certificate (HNC)"),
                };
                return Ok(entries);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting qualification levels");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("IndustrySectors")]
        public async Task<IActionResult> GetIndustrySectors()
        {
            try
            {
                var entries = new List<DictionaryEntry> {
                    new DictionaryEntry("soc1", "Sector1"),
                    new DictionaryEntry("soc2", "Sector2"),
                    new DictionaryEntry("soc3", "Sector3"),
                };
                return Ok(entries);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting industry sectors");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("EstimatedWage")]
        public async Task<IActionResult> GetEstimatedWage(string socCode)
        {
            try
            {
                return Ok((decimal)123.45);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting estimated wage data");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("LivingWageValues")]
        public async Task<IActionResult> GetLivingWageValues()
        {
            try
            {
                return Ok((decimal)234.56);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting living wage data");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
