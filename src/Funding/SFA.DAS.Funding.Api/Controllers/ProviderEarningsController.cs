using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;

namespace SFA.DAS.Funding.Api.Controllers
{
    [ApiController]
    public class ProviderEarningsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderEarningsController> _logger;

        public ProviderEarningsController(IMediator mediator, ILogger<ProviderEarningsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("/{ukprn}/summary")]
        public async Task<IActionResult> GetSummary(long ukprn)
        {
            var result = await _mediator.Send(new GetProviderEarningsSummaryQuery
            {
                Ukprn = ukprn
            });

            var response = new GetProviderEarningsSummaryResponse { Summary = result.Summary };

            return Ok(response);
        }

        [HttpGet]
        [Route("/{ukprn}/GenerateCSV")]
        public async Task<IActionResult> GenerateCSV(long ukprn)
        {
            var result = await _mediator.Send(new GetProviderAcademicYearEarningsQuery
            {
                Ukprn = ukprn
            });

            var response = new GetProviderAcademicYearEarningsResponse { AcademicYearEarnings = result.AcademicYearEarnings };

            return Ok(response);
        }
    }
}
