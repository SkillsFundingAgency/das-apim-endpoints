using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;
using SFA.DAS.NServiceBus;
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
                    RplDataExtended = request.RplDataExtended,
                    CsvRecords = request.CsvRecords?.ToList(),
                    UserInfo = request.UserInfo
                });

            return Ok(result);
        }

        [HttpPost]
        [Route("AddAndApprove")]
        public async Task<IActionResult> AddAndApprove(BulkUploadAddAndApproveDraftApprenticeshipsRequest request)
        {
            var result = await _mediator.Send(
                new BulkUploadAddAndApproveDraftApprenticeshipsCommand
                {
                    ProviderId = request.ProviderId,
                    BulkUploadAddAndApproveDraftApprenticeships = request.BulkUploadAddAndApproveDraftApprenticeships?.ToList(),
                    UserInfo = request.UserInfo
                });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddDraftapprenticeships(BulkUploadAddDraftApprenticeshipsRequest request)
        {
            var result = await _mediator.Send(
                new BulkUploadAddDraftApprenticeshipsCommand
                {
                    ProviderId = request.ProviderId,
                    RplDataExtended = request.RplDataExtended,
                    BulkUploadAddDraftApprenticeships = request.BulkUploadDraftApprenticeships?.ToList(),
                    UserInfo = request.UserInfo
                });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddLog")]
        public async Task<IActionResult> AddLog(BulkUploadAddLogRequest request)
        {
            var result = await _mediator.Send(
                new BulkUploadAddLogCommand
                {
                    ProviderId = request.ProviderId,
                    FileName = request.FileName,
                    RplCount = request.RplCount,
                    RowCount = request.RowCount,
                    FileContent = request.FileContent
                });

            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateLog")]
        public async Task<IActionResult> UpdateLog(BulkUploadUpdateLogRequest request)
        {
            var result = await _mediator.Send(
                new BulkUploadUpdateLogCommand
                {
                    LogId = request.LogId
                });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}