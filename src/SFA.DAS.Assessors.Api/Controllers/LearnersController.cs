using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Assessors.Application.Queries.GetAllLearners;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class LearnersController : ControllerBase
    {
        private readonly ILogger<LearnersController> _logger;
        private readonly IMediator _mediator;

        public LearnersController(ILogger<LearnersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllLearners(DateTime? sinceTime = null, int batch_number = 1, int batch_size = 1000)
        {
            try
            {
                var result = await _mediator.Send(new GetAllLearnersQuery(sinceTime, batch_number, batch_size));

                return Ok(new GetAllLearnersResponse()
                {
                    Learners = result.Learners,
                    BatchNumber = result.BatchNumber,
                    BatchSize = result.BatchSize,
                    TotalNumberOfBatches = result.TotalNumberOfBatches
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting learners");
                return BadRequest();
            }
        }
    }
}