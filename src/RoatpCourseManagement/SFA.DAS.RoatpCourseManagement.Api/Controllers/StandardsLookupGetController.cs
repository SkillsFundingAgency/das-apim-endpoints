using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardsLookup;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    public class StandardsLookupGetController : ControllerBase
    {
        private readonly ILogger<StandardsLookupGetController> _logger;
        private readonly IMediator _mediator;

        public StandardsLookupGetController(ILogger<StandardsLookupGetController> logger, IMediator mediator)
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

        [HttpGet]
        [Route("lookup/standards/{larsCode}")]
        public async Task<ActionResult<GetStandardInformationQueryResult>> GetStandardInformation([FromRoute] int larsCode)
        {
            _logger.LogInformation("Outer API: request received to get details for standard: {larscode} from courses api", larsCode);
            return await _mediator.Send(new GetStandardInformationQuery(larsCode));
        }
    }
}
