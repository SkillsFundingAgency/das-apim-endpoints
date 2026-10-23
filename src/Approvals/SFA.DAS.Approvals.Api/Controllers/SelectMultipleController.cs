using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Commands;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class SelectMultipleController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [Route("Validate")]
    public async Task<IActionResult> Validate(SelectMultipleValidateApimRequest request)
    {
        var result = await mediator.Send(
            new ValidateSelectMultipleLearnerRecordsQuery
            {
                ProviderId = request.ProviderId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                AgreementId = request.AgreementId,
                LearnerIds = request.LearnerIds,
                UserInfo = request.UserInfo,
            });

        return Ok(result);
    }

    [HttpPost]
    [Route("AddDraftApprenticeships")]
    public async Task<IActionResult> AddDraftApprenticeships(SelectMultipleAddDraftApprenticeshipsRequest request)
    {
        var result = await mediator.Send(
            new SelectMultipleAddDraftApprenticeshipsCommand
            {
                ProviderId = request.ProviderId,
                UserInfo = request.UserInfo,
                AccountLegalEntityId = request.AccountLegalEntityId,
                AgreementId = request.AgreementId,
                AccountId = request.AccountId,
                LearnerIds = request.LearnerIds
            });

        return Ok(result);
    }
}
