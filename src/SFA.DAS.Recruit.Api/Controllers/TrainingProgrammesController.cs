using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingProgrammesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TrainingProgrammesController> _logger;

        public TrainingProgrammesController (IMediator mediator, ILogger<TrainingProgrammesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingProgrammesQuery());
                
                var response = new GetTrainingProgrammesListResponse
                {
                    TrainingProgrammes = result.TrainingProgrammes.Select(c=>(GetTrainingProgrammeResponse)c)
                };
                
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting list of training programmes");
                return BadRequest();
            }
        }
    }
}
