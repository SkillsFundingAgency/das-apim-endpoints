using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardsLookup;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    public class GetStandardsLookupController : ControllerBase
    {
        private readonly ILogger<GetStandardsLookupController> _logger;
        private readonly IMediator _mediator;

        public GetStandardsLookupController(ILogger<GetStandardsLookupController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("lookup/standards")]
        public async Task<IActionResult> GetAllStandards()
        {
            _logger.LogInformation("Get all active standards");
            var result = await _mediator.Send(new GetStandardsLookupQuery());

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"Active standards not gathered, status code {result.StatusCode}, Error content:[{result.ErrorContent}]");
                return StatusCode((int)result.StatusCode, result.ErrorContent);
            }

            _logger.LogInformation("Active standards gathered");
            return Ok(result.Body);
        }
    }
}
