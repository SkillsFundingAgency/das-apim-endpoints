using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
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
        [Route("[controller]/statuses")]
        public async Task<IActionResult> GetDataLockStatuses(long sinceEventId, DateTime? sinceTime, string employerAccountId, long ukprn, int page = 1)
        {
            try
            {
                var response = await _mediator.Send(new GetDataLockStatuesQuery()
                {
                    EmployerAccountId = employerAccountId,
                    SinceEventId = sinceEventId,
                    PageNumber = page,
                    SinceTime = sinceTime,
                    Ukprn = ukprn
                });

                var model = new GetDataLockStatusListResponse
                {
                    TotalNumberOfPages = response?.PagedDataLockEvent.TotalNumberOfPages ?? 0,
                    DataLockStatuses = response?.PagedDataLockEvent?.Items?.Select(c => (GetDataLockStatusResponse)c)
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting DataLockEvents");
                return BadRequest();
            }
        }
    }
}