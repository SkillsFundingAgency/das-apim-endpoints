using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship
{
    public class AddDraftApprenticeshipCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        IAutoReservationsService autoReservationService,
        ICourseTypeRulesService courseTypeRulesService,
        ILogger<AddDraftApprenticeshipCommandHandler> logger)
        : IRequestHandler<AddDraftApprenticeshipCommand, AddDraftApprenticeshipResult>
    {
        public async Task<AddDraftApprenticeshipResult> Handle(AddDraftApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var autoReservationCreated = false;
            
            var cohort = await apiClient.Get<GetCohortResponse>(new GetCohortRequest(request.CohortId));

            if (!request.ReservationId.HasValue || request.ReservationId.Value == Guid.Empty)
            {
                if (cohort.TransferSenderId.HasValue)
                {
                    throw new ApplicationException("When creating an auto reservation, the TransferSenderId must not present");
                }

                request.ReservationId = await autoReservationService.CreateReservation(new AutoReservation
                {
                    AccountId = cohort.AccountId,
                    AccountLegalEntityId = cohort.AccountLegalEntityId,
                    CourseCode = request.CourseCode,
                    StartDate = request.StartDate,
                    UserInfo = request.UserInfo
                });
                autoReservationCreated = true;
            }

            try
            {
                var courseTypeRules = await courseTypeRulesService.GetCourseTypeRulesAsync(request.CourseCode);
                
                var addDraftApprenticeshipRequest = new AddDraftApprenticeshipRequest
                {
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
                    ProviderId = request.ProviderId,
                    ReservationId = request.ReservationId,
                    StartDate = request.StartDate,
                    Uln = request.Uln,
                    UserInfo = request.UserInfo,
                    UserId = request.UserId,
                    RequestingParty = request.RequestingParty,
                    LearnerDataId = request.LearnerDataId,
                    MinimumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MinimumAge,
                    MaximumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MaximumAge,
                };
                var response = await apiClient.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                    new PostAddDraftApprenticeshipRequest(request.CohortId, addDraftApprenticeshipRequest));

                return new AddDraftApprenticeshipResult
                {
                    DraftApprenticeshipId = response.Body.DraftApprenticeshipId
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