using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class SelectMultipleController(
    IMediator mediator,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient) : ControllerBase
{
    private const int PendingReservationStatus = 0;

    [HttpGet]
    [Route("AvailableReservations/{accountId:long}/{accountLegalEntityId:long}")]
    public async Task<IActionResult> AvailableReservations(long accountId, long accountLegalEntityId)
    {
        var response = await reservationApiClient.Get<List<GetAccountReservationsResponse>>(
            new GetAccountReservationsRequest(accountId));

        var reservations = response
            .Where(x => x.AccountLegalEntityId == accountLegalEntityId &&
                        x.Status == PendingReservationStatus &&
                        x.TransferSenderAccountId == null &&
                        (!x.ExpiryDate.HasValue || x.ExpiryDate.Value >= DateTime.UtcNow))
            .Select(x => new SelectMultipleAvailableReservation
            {
                Id = x.Id,
                AccountLegalEntityId = x.AccountLegalEntityId,
                StartDate = x.StartDate,
                ExpiryDate = x.ExpiryDate
            })
            .ToList();

        return Ok(reservations);
    }

    [HttpPost]
    [Route("Validate")]
    public async Task<IActionResult> Validate(SelectMultipleValidateApimRequest request)
    {
        var result = await mediator.Send(
            new ValidateSelectMultipleLearnerRecordsQuery
            {
                ProviderId = request.ProviderId,
                AccountId = request.AccountId,
                IsLevyEmployer = request.IsLevyEmployer,
                AccountLegalEntityId = request.AccountLegalEntityId,
                AgreementId = request.AgreementId,
                LearnerIds = request.LearnerIds,
                ReservationAssignments = request.ReservationAssignments,
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
                LearnerIds = request.LearnerIds,
                ReservationAssignments = request.ReservationAssignments
            });

        return Ok(result);
    }
}
