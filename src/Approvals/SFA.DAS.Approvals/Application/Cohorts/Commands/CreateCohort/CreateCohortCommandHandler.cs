﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort
{
    public class CreateCohortCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        IAutoReservationsService autoReservationService,
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ITrainingTypesApiClient trainingTypesApiClient,
        ILogger<CreateCohortCommandHandler> logger)
        : IRequestHandler<CreateCohortCommand, CreateCohortResult>
    {
        public async Task<CreateCohortResult> Handle(CreateCohortCommand request, CancellationToken cancellationToken)
        {
            var autoReservationCreated = false;

            if (!request.ReservationId.HasValue || request.ReservationId.Value == Guid.Empty)
            {
                if (request.TransferSenderId != null)
                {
                    throw new ApplicationException("When creating a auto reservation, the TransferSenderId must be null");
                }

                request.ReservationId = await autoReservationService.CreateReservation(new AutoReservation
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
                var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(request.CourseCode));
                
                if (standard == null)
                {
                    logger.LogError("Standard not found for course ID {CourseId}", request.CourseCode);
                    throw new Exception($"Standard not found for course ID {request.CourseCode}");
                }
                
                var learnerAge = await trainingTypesApiClient.Get<GetLearnerAgeResponse>(new GetLearnerAgeRequest(standard.ApprenticeshipType));

                if (learnerAge == null)
                {
                    logger.LogError("Learner age rules not found for apprenticeship type {ApprenticeshipType}", standard.ApprenticeshipType);
                    throw new Exception($"Learner age rules not found for apprenticeship type {standard.ApprenticeshipType}");
                }

                var createCohortRequest = new CreateCohortRequest
                {
                    AccountId = request.AccountId,
                    AccountLegalEntityId = request.AccountLegalEntityId,
                    ProviderId = request.ProviderId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    Uln = request.Uln,
                    CourseCode = request.CourseCode,
                    DeliveryModel = request.DeliveryModel,
                    Cost = request.Cost,
                    TrainingPrice = request.TrainingPrice,
                    EndPointAssessmentPrice = request.EndPointAssessmentPrice,
                    StartDate = request.StartDate,
                    ActualStartDate = request.ActualStartDate,
                    EndDate = request.EndDate,
                    OriginatorReference = request.OriginatorReference,
                    ReservationId = request.ReservationId,
                    TransferSenderId = request.TransferSenderId,
                    PledgeApplicationId = request.PledgeApplicationId,
                    EmploymentPrice = request.EmploymentPrice,
                    EmploymentEndDate = request.EmploymentEndDate,
                    IgnoreStartDateOverlap = request.IgnoreStartDateOverlap,
                    IsOnFlexiPaymentPilot = request.IsOnFlexiPaymentPilot,
                    LearnerDataId = request.LearnerDataId,
                    MaximumAgeAtApprenticeshipStart = learnerAge.MaximumAge,
                    UserInfo = request.UserInfo,
                    RequestingParty = request.RequestingParty
                };

                var createCohortResponse = await apiClient.PostWithResponseCode<CreateCohortResponse>(new PostCreateCohortRequest(createCohortRequest));

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
                    await autoReservationService.DeleteReservation(request.ReservationId.Value);
                }
                throw;
            }
        }
    }
}