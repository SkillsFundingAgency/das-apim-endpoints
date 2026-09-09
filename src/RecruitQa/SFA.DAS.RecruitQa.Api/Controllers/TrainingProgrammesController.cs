using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;

namespace SFA.DAS.RecruitQa.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingProgrammesController(IMediator mediator, ILogger<TrainingProgrammesController> logger)
        : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll([FromQuery] int? ukprn = null)
        {
            try
            {
                var result = await mediator.Send(new GetTrainingProgrammesQuery(ukprn));

                var response = new GetTrainingProgrammesListResponse
                {
                    TrainingProgrammes = result._.Select(c => (GetTrainingProgrammeResponse)c)
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting list of training programmes");
                return BadRequest();
            }
        }
    }
}