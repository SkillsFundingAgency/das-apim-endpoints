using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    public class CollectionCalendarController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CollectionCalendarController> _logger;

        public CollectionCalendarController(IMediator mediator, ILogger<CollectionCalendarController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPatch]
        [Route("/collectionCalendar/period/active")]
        public async Task<IActionResult> ActivateCollectionCalendarPeriod([FromBody] ActivateCollectionCalendarPeriodRequest request)
        {
            await _mediator.Send(new ActivateCollectionCalendarPeriodCommand(request.CollectionPeriodNumber, 
                                                                             request.CollectionPeriodYear,
                                                                             request.Active));

            return new OkResult();
        }
    }
}
