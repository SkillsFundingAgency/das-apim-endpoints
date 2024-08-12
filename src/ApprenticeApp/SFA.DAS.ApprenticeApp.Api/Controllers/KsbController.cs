using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SFA.DAS.ApprenticeApp.Models;
using System.Linq;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/ksb/{id}/options/{option}/ksbs")]

        public async Task<IActionResult> GetKsbs(string id, string option)
        {
            var queryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = id,
                Option = option
            });

            return Ok(queryResult.KsbsResult);
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
    }
}
