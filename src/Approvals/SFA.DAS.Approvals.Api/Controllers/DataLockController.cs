using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class DataLockController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DataLockController> _logger;

        public DataLockController(IMediator mediator, ILogger<DataLockController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/events")]
        public async Task<IActionResult> GetDataLockEvents(GetDataLockEventsRequest request)
        {
            try
            {
                var response = await _mediator.Send(new GetDataLockEventsQuery()
                {
                    EmployerAccountId = request.EmployerAccountId,
                    SinceEventId = request.SinceEventId,
                    PageNumber = request.PageNumber,
                    SinceTime = request.SinceTime,
                    Ukprn = request.Ukprn
                });

                return Ok(response.PagedDataLockEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting DataLockEvents");
                return BadRequest();
            }
        }
    }
}