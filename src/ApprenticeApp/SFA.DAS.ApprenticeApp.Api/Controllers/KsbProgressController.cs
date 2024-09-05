using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbProgressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbProgressController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("/apprentices/{apprenticeshipId}/ksbs")]
        public async Task<IActionResult> AddUpdateKsbProgress(long apprenticeshipId, ApprenticeKsbProgressData data)
        {
            await _mediator.Send(new AddUpdateKsbProgressCommand
            {
                ApprenticeshipId = apprenticeshipId,
                Data = data
            });

            return Ok();
        }

        // remove the ksb to task association
        [HttpDelete("/apprentices/{apprenticeshipId}/ksbs/{ksbProgressId}/taskid/{taskId}")]
        public async Task<IActionResult> RemoveTaskToKsbProgress(long apprenticeshipId, int ksbProgressId, int taskId)
        {
            await _mediator.Send(new RemoveTaskToKsbProgressCommand
            {
                TaskId = taskId,
                KsbProgressId = ksbProgressId,
                ApprenticeshipId = apprenticeshipId
            });

            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeshipId}/ksbs")]
        public async Task<IActionResult> GetKsbsByApprenticeshipIdAndGuidListQuery(long apprenticeshipId, [FromQuery] Guid[] guids)
        {
            var queryResult = await _mediator.Send(new GetKsbsByApprenticeshipIdAndGuidListQuery
            {
                ApprenticeshipId = apprenticeshipId,
                Guids = guids
            });

            return Ok(queryResult.KSBProgresses);
        }

        [HttpGet]
        [Route("/apprentices/{apprenticeshipId}/apprenticeship/{standardUid}/options/{option}/ksbs")]
        public async Task<IActionResult> GetApprenticeshipKsbs(long apprenticeshipId, string standardUid, string option)
        {
            var ksbQueryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = standardUid,
                Option = option
            });

            if (ksbQueryResult.KsbsResult != null && ksbQueryResult.KsbsResult.Ksbs.Count > 0)
            {
                var ksbProgressResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery { ApprenticeshipId = apprenticeshipId });

                var apprenticeKsbs = new List<ApprenticeKsb>();
                foreach (var ksb in ksbQueryResult.KsbsResult.Ksbs)
                {
                    var apprenticeKsb = new ApprenticeKsb()
                    {
                        Id = ksb.Id,
                        Key = ksb.Key,
                        Detail = ksb.Detail,
                        Type = ksb.Type
                    };

                    var ksbProgress = ksbProgressResult.KSBProgresses.FirstOrDefault(x => x.KSBId == ksb.Id);
                    if (ksbProgress != null)
                    {
                        apprenticeKsb.Progress = ksbProgress;
                    }
                    apprenticeKsbs.Add(apprenticeKsb);
                }

                return Ok(apprenticeKsbs);
            }
            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeshipId}/ksbs/taskid/{taskId}")]
        public async Task<IActionResult> GetKsbProgressForTask(long apprenticeshipId, int taskId)
        {
            var queryResult = await _mediator.Send(new GetKsbProgressForTaskQuery
            {
                ApprenticeshipId = apprenticeshipId,
                TaskId = taskId
            });

            return Ok(queryResult.KSBProgress);
        }

        [HttpGet]
        [Route("/apprentices/{apprenticeshipId}/apprenticeship/{standardUid}/options/{option}/ksb/{ksbId}")]
        public async Task<IActionResult> GetApprenticeshipKsbProgress(long apprenticeshipId, string standardUid, string option, Guid ksbId)
        {
            var ksbQueryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = standardUid,
                Option = option
            });

            if (ksbQueryResult.KsbsResult != null && ksbQueryResult.KsbsResult.Ksbs.Count > 0)
            {
                var ksbProgressResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery { ApprenticeshipId = apprenticeshipId });

                var ksb = ksbQueryResult.KsbsResult.Ksbs.Where(k => k.Id == ksbId).FirstOrDefault();
                if(ksb != null)
                    {
                        var apprenticeKsb = new ApprenticeKsb()
                        {
                            Id = ksb.Id,
                            Key = ksb.Key,
                            Detail = ksb.Detail,
                            Type = ksb.Type
                        };

                        var ksbProgress = ksbProgressResult.KSBProgresses.FirstOrDefault(x => x.KSBId == ksb.Id);
                        if (ksbProgress != null)
                        {
                            apprenticeKsb.Progress = ksbProgress;
                        }
                        return Ok(apprenticeKsb);
                    }
            }
            return Ok();
        }
    }
}