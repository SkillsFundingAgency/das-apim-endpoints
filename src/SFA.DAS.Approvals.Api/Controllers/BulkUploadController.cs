using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class BulkUploadController : Controller
    {
        private readonly ILogger<DraftApprenticeshipController> _logger;
        private readonly IMediator _mediator;

        public BulkUploadController(ILogger<DraftApprenticeshipController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Validate")]
        public async Task<IActionResult> Validate(BulkUploadValidateApimRequest request)
        {
            var result = await _mediator.Send(
                new ValidateBulkUploadRecordsCommand
                {
                    ProviderId = request.ProviderId,
                    CsvRecords = request.CsvRecords.ToList(),
                    UserInfo = request.UserInfo
                });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
