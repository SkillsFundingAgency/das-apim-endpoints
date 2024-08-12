using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbProgressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbProgressController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("/apprentices/{id}/ksbs")]
        public async Task<IActionResult> AddUpdateKsbProgress(Guid id, ApprenticeKsbProgressData data)
        {
            await _mediator.Send(new AddUpdateKsbProgressCommand
            {
                ApprenticeshipId = id,
                Data = data
            });

            return Ok();
        }

        // remove the ksb to task association
        [HttpDelete("/apprentices/{id}/ksbs/{ksbProgressId}/taskid/{taskId}")]
        public async Task<IActionResult> RemoveTaskToKsbProgress(Guid id, int ksbProgressId, int taskId)
        {
            await _mediator.Send(new RemoveTaskToKsbProgressCommand
            {
                TaskId = taskId,
                KsbProgressId = ksbProgressId,
                ApprenticeshipId = id
            });

            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeshipIdentifier}/ksbs")]
        public async Task<IActionResult> GetKsbsByApprenticeshipIdAndGuidListQuery(Guid apprenticeshipIdentifier, [FromQuery] Guid[] guids)
        {
            var queryResult = await _mediator.Send(new GetKsbsByApprenticeshipIdAndGuidListQuery
            {
                ApprenticeshipId = apprenticeshipIdentifier,
                Guids = guids
            });

            return Ok(queryResult.KSBProgresses);
        }

        [HttpGet("/apprentices/{apprenticeshipIdentifier}/ksbs")]
        public async Task<IActionResult> GetKsbsByApprenticeshipId(Guid apprenticeshipIdentifier)
        {
            var queryResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery
            {
                ApprenticeshipId = apprenticeshipIdentifier
            });

            return Ok(queryResult.KSBProgresses);
        }

        [HttpGet]
        [Route("/apprentices/{apprenticeAccountId}/apprenticeship/{standardUid}/options/{option}/ksbs")]
        public async Task<IActionResult> GetApprenticeshipKsbs(Guid apprenticeAccountId, string standardUid, string option)
        {
            var ksbQueryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = standardUid,
                Option = option
            });

            if (ksbQueryResult.KsbsResult != null && ksbQueryResult.KsbsResult.Ksbs.Count > 0)
            {
                var ksbProgressResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery { ApprenticeshipId = apprenticeAccountId });

                var apprenticeKsbs = new List<ApprenticeKsb>();
                foreach (var ksb in ksbQueryResult.KsbsResult.Ksbs)
                {
                    var apprenticeKsb = new ApprenticeKsb
                    {
                        Id = ksb.Id,
                        Type = ksb.Type,
                        Key = ksb.Key,
                        Detail = ksb.Detail,
                        Status = ksbProgressResult.KSBProgresses.Where(x => x.KSBId == ksb.Id).Select(x => x.CurrentStatus).DefaultIfEmpty(KSBStatus.NotStarted).First()
                    };
                    apprenticeKsbs.Add(apprenticeKsb);
                }
                return Ok(apprenticeKsbs);
            }
            return Ok();
        }
    }
}
