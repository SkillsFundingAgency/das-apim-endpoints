using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Telemetry;
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
        private readonly IApprenticeAppMetrics _apprenticeAppMetrics;
        public KsbProgressController(IMediator mediator)
            => _mediator = mediator;
        public KsbProgressController(IApprenticeAppMetrics metrics)
        {
            _apprenticeAppMetrics = metrics;
        }

        
        [HttpPost("/apprentices/{apprenticeId}/ksbs")]
        public async Task<IActionResult> AddUpdateKsbProgress(Guid apprenticeId, ApprenticeKsbProgressData data)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            await _mediator.Send(new AddUpdateKsbProgressCommand
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                Data = data
            });
            _apprenticeAppMetrics.IncreaseKSBInProgress(apprenticeshipId.ToString(), data.ToString());
            return Ok();
        }

        // remove the ksb to task association
        [HttpDelete("/apprentices/{apprenticeId}/ksbs/{ksbProgressId}/taskid/{taskId}")]
        public async Task<IActionResult> RemoveTaskToKsbProgress(Guid apprenticeId, int ksbProgressId, int taskId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            await _mediator.Send(new RemoveTaskToKsbProgressCommand
            {
                TaskId = taskId,
                KsbProgressId = ksbProgressId,
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId
            });

            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeId}/ksbs/guids")]
        public async Task<IActionResult> GetKsbsByApprenticeshipIdAndGuidListQuery(Guid apprenticeId, [FromQuery] Guid[] guids)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var queryResult = await _mediator.Send(new GetKsbsByApprenticeshipIdAndGuidListQuery
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                Guids = guids
            });
            _apprenticeAppMetrics.IncreaseKSBsViews(apprenticeshipId.ToString());
            return Ok(queryResult.KSBProgresses);
        }

        [HttpGet]
        [Route("/apprentices/{apprenticeId}/ksbs")]
        public async Task<IActionResult> GetApprenticeshipKsbs(Guid apprenticeId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var ksbQueryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.StandardUId,
                Option = "core" //to be updated when commitments api added
            });

            if (ksbQueryResult.KsbsResult != null && ksbQueryResult.KsbsResult.Ksbs.Count > 0)
            {
                var ksbProgressResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery { ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId });

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
                _apprenticeAppMetrics.IncreaseKSBsViews(standardUid);
                return Ok(apprenticeKsbs);
            }
            
            return Ok();
        }

        [HttpGet("/apprentices/{apprenticeId}/ksbs/taskid/{taskId}")]
        public async Task<IActionResult> GetKsbProgressForTask(Guid apprenticeId, int taskId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var queryResult = await _mediator.Send(new GetKsbProgressForTaskQuery
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                TaskId = taskId
            });

            return Ok(queryResult.KSBProgress);
        }

        [HttpGet]
        [Route("/apprentices/{apprenticeId}/ksbs/{ksbId}")]
        public async Task<IActionResult> GetApprenticeshipKsbProgress(Guid apprenticeId, Guid ksbId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var ksbQueryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.StandardUId,
                Option = "core"
            });

            if (ksbQueryResult.KsbsResult != null && ksbQueryResult.KsbsResult.Ksbs.Count > 0)
            {
                var ksbProgressResult = await _mediator.Send(new GetKsbsByApprenticeshipIdQuery { ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId });

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