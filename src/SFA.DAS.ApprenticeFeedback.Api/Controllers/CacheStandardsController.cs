using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeFeedback.Api.Models;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetStandards;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    public class CacheStandardsController : Controller
    {
        private readonly IMediator _mediator;

        public CacheStandardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CacheStandards()
        {
            var standards = await _mediator.Send(new GetStandardsQuery());

            var command = new CacheStandardsCommand
            {
                Standards = standards.Standards.Select(s => (Standard)s)
            };

            var cacheResponse = 

            return Ok(response);
        }
    }
}
