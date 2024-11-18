﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort
{
    public class CreateCohortCommandHandler : IRequestHandler<CreateCohortCommand, CreateCohortResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IAutoReservationsService _autoReservationService;
        private readonly IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient;

        public CreateCohortCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IAutoReservationsService autoReservationService)
        {
            _apiClient = apiClient;
            _autoReservationService = autoReservationService;
        }

        public async Task<CreateCohortResult> Handle(CreateCohortCommand request, CancellationToken cancellationToken)
        {
            var autoReservationCreated = false;

            if (!request.ReservationId.HasValue || request.ReservationId.Value == default)
            {
                if (request.TransferSenderId != null)
                {
                    throw new ApplicationException("When creating a auto reservation, the TransferSenderId must be null");
                }

                request.ReservationId = await _autoReservationService.CreateReservation(new AutoReservation
                {
                    AccountId = request.AccountId,
                    AccountLegalEntityId = request.AccountLegalEntityId,
                    CourseCode = request.CourseCode,
                    StartDate = request.StartDate,
                    UserInfo = request.UserInfo

                });
                autoReservationCreated = true;
            }

            try
            {
                var createCohortRequest = new CreateCohortRequest
                {
                    AccountId = request.AccountId,
                    AccountLegalEntityId = request.AccountLegalEntityId,
                    ActualStartDate = request.ActualStartDate,
                    Cost = request.Cost,
                    TrainingPrice = request.TrainingPrice,
                    EndPointAssessmentPrice = request.EndPointAssessmentPrice,
                    CourseCode = request.CourseCode,
                    DateOfBirth = request.DateOfBirth,
                    DeliveryModel = request.DeliveryModel,
                    Email = request.Email,
                    EmploymentEndDate = request.EmploymentEndDate,
                    EmploymentPrice = request.EmploymentPrice,
                    EndDate = request.EndDate,
                    FirstName = request.FirstName,
                    IgnoreStartDateOverlap = request.IgnoreStartDateOverlap,
                    IsOnFlexiPaymentPilot = request.IsOnFlexiPaymentPilot,
                    LastName = request.LastName,
                    OriginatorReference = request.OriginatorReference,
                    PledgeApplicationId = request.PledgeApplicationId,
                    ProviderId = request.ProviderId,
                    ReservationId = request.ReservationId,
                    StartDate = request.StartDate,
                    TransferSenderId = request.TransferSenderId,
                    Uln = request.Uln,
                    UserInfo = request.UserInfo,
                    RequestingParty = request.RequestingParty
                };

                var createCohortResponse =
                    await _apiClient.PostWithResponseCode<CreateCohortResponse>(
                        new PostCreateCohortRequest(createCohortRequest));

                return new CreateCohortResult
                {
                    CohortId = createCohortResponse.Body.CohortId,
                    CohortReference = createCohortResponse.Body.CohortReference
                };
            }
            catch
            {
                if (autoReservationCreated)
                {
                    await _autoReservationService.DeleteReservation(request.ReservationId.Value);
                }
                throw;
            }
        }
    }
}