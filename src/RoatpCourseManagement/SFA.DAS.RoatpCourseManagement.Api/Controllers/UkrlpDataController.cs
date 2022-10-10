using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Services;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using NLog;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class UkrlpDataController : ControllerBase
    {
        private readonly IUkrlpService _ukrlpService;
        private readonly ILogger<UkrlpDataController> _logger;

        public UkrlpDataController(IUkrlpService ukrlpService, ILogger<UkrlpDataController> logger)
        {
            _ukrlpService = ukrlpService;
            _logger = logger;
        }

        [HttpPost]
        [Route("lookup/providers-address")]
        public async Task<IActionResult> GetProvidersData(UkrlpDataCommand command)
        {
            if (command.ProvidersUpdatedSince == null && !command.Ukprns.Any())
            {
                _logger.LogWarning("No parameter entered for providers address lookup");
                return BadRequest();
            }

            if (command.ProvidersUpdatedSince != null && command.Ukprns.Any())
            {
                _logger.LogWarning("Two parameters entered for providers address lookup");
                return BadRequest();
            }

            _logger.LogInformation("Request to retrieve course directory data received");
            var results = await _ukrlpService.GetAddresses(command);
            if (results == null)
            {
                _logger.LogWarning("No results returned from ukrlpService");
                return NotFound();
            }

            return Ok(results);
        }
    }
}
