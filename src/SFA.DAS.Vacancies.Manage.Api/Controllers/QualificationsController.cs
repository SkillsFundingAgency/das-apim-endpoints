using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Manage.Application.Qualifications.Queries.GetQualifications;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class QualificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<QualificationsController> _logger;

        public QualificationsController (IMediator mediator, ILogger<QualificationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetList([FromRoute]int ukprn)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetQualificationsQuery
                {
                    Ukprn = ukprn
                });

                return Ok((GetQualificationsQueryResponse) queryResponse);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get qualifications [{ukprn}]");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}