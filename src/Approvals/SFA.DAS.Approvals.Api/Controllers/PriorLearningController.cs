using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class PriorLearningController : Controller
    {
        private readonly ILogger<PriorLearningController> _logger;
        private readonly IMediator _mediator;

        public PriorLearningController(ILogger<PriorLearningController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("priorlearningdata/{cohortId}/{draftApprenticeshipId}")]
        public async Task<IActionResult> PriorLearningData(long cohortId, long draftApprenticeshipId, [FromBody] AddPriorLearningDataRequest request)
        {
            var command = new AddPriorLearningDataCommand
            {
                CohortId = cohortId,
                DraftApprenticeshipId = draftApprenticeshipId,

                CostBeforeRpl = request.CostBeforeRpl,
                DurationReducedBy= request.DurationReducedBy,
                DurationReducedByHours= request.DurationReducedByHours,
                IsDurationReducedByRpl= request.IsDurationReducedByRpl,
                PriceReducedBy= request.PriceReducedBy,
                TrainingTotalHours = request.TrainingTotalHours                
            };

            await _mediator.Send(command);

            return Ok();
        }
    }
}
