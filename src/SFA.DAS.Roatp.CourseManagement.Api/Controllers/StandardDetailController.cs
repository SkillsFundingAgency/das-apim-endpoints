using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StandardDetailController : ControllerBase
    {
        private readonly ILogger<StandardDetailController> _logger;
        private readonly IMediator _mediator;

        public StandardDetailController(ILogger<StandardDetailController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{larsCode}")]
        public async Task<IActionResult> GetStandardDetail(int larsCode)
        {
            if (larsCode <= 0)
            {
                _logger.LogInformation("Invalid lars code {larsCode}", larsCode);
                return BadRequest();
            }

            _logger.LogInformation("Get Standard for lars code {larsCode}", larsCode);
            try
            {
                var result = await _mediator.Send(new GetStandardQuery(larsCode));

                if (result == null)
                {
                    _logger.LogInformation("Standard data not found for lars code {larsCode}", larsCode);
                    return NotFound();
                }

                _logger.LogInformation("Standard data found for lars code {larsCode}", larsCode);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve Standard data for lars code {larsCode}");
                return BadRequest();
            }
        }
    }
}
