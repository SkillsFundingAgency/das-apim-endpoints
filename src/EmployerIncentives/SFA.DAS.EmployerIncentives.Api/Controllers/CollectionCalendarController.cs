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
        [Route("/collectionPeriods")]
        public async Task<IActionResult> UpdateCollectionCalendarPeriod([FromBody] UpdateCollectionCalendarPeriodRequest request)
        {
            await _mediator.Send(new UpdateCollectionCalendarPeriodCommand(request.PeriodNumber, 
                                                                           request.AcademicYear,
                                                                           request.Active));

            return new OkResult();
        }
    }
}
