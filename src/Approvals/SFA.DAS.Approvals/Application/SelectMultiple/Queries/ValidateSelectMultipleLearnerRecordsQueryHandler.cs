using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Validation;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Queries;

public class ValidateSelectMultipleLearnerRecordsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    IProviderCoursesOrStandardsService providerCoursesService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateSelectMultipleLearnerRecordsQuery, ValidateSelectMultipleLearnerRecordsQueryResult>
{
    public async Task<ValidateSelectMultipleLearnerRecordsQueryResult> Handle(ValidateSelectMultipleLearnerRecordsQuery command, CancellationToken cancellationToken)
    {
        var learnerIds = command.LearnerIds?.Distinct().ToList() ?? [];

        if (learnerIds.Count == 0)
        {
            throw new ArgumentException("LearnerIds must not be empty", nameof(command.LearnerIds));
        }

        var learnerDataResponse = await learnerDataClient.PostWithResponseCode<List<LearnerDataRecord>>(
           new PostGetLearnersForProviderByIdsRequest(
               command.ProviderId, new GetLearnersForProviderByIdsRequest
               {
                   LearnerIds = learnerIds
               }
           ));

        if (!string.IsNullOrEmpty(learnerDataResponse.ErrorContent))
        {
            throw new InvalidOperationException($"Getting Learner Data Failed, Status Code {learnerDataResponse.StatusCode} Error : {learnerDataResponse.ErrorContent}");
        }

        if (learnerDataResponse.Body == null || learnerDataResponse.Body.Count != learnerIds.Count)
        {
            throw new InvalidOperationException($"Getting Learner Data Failed, expected {learnerIds.Count} learners but received {learnerDataResponse.Body?.Count ?? 0}");
        }

        var assignments = command.ReservationAssignments?.ToList();
        var newLearnerIds = assignments != null && assignments.Count == 0
            ? []
            : assignments == null
            ? learnerIds
            : assignments.Where(x => x.UseNewReservation).Select(x => x.LearnerId).ToList();

        if (!command.IsLevyEmployer && assignments?.Count > 0)
        {
            var reservationStatus = await reservationApiClient.Get<GetAccountReservationsStatusResponse>(
                new GetAccountReservationsStatusRequest(command.AccountId));
            if (newLearnerIds.Count > reservationStatus.RemainingReservationsCount)
            {
                var message = $"You can reserve new funds for {reservationStatus.RemainingReservationsCount} learners. Remove learners or assign existing reservations.";
                ThrowDomainErrors(
                [
                    new ErrorDetail("Selection", message),
                    new ErrorDetail("Assignments", message)
                ]);
            }
        }

        if (assignments != null && assignments.Count > 0)
        {
            await ValidateExistingReservations(command, learnerDataResponse.Body, assignments);
        }

        var reservationRequests = learnerDataResponse.Body
            .Where(learner => newLearnerIds.Contains(learner.Id))
            .Select((learner, index) =>
        {
            return new ReservationRequest
            {
                CourseId = learner.TrainingCode,
                AccountLegalEntityId = command.AccountLegalEntityId ?? 0,
                ProviderId = (uint?)command.ProviderId,
                RowNumber = index + 1,
                Id = Guid.NewGuid(),
                StartDate = learner.StartDate,
            };

        }).ToList();
        BulkReservationValidationResults reservationValidationResult;
        if (reservationRequests.Count == 0)
        {
            reservationValidationResult = new BulkReservationValidationResults { ValidationErrors = [] };
        }
        else
        {
            var reservationResponse = await reservationApiClient.PostWithResponseCode<BulkReservationValidationResults>(
                new PostValidateReservationRequest(command.ProviderId, reservationRequests));

            if (!string.IsNullOrEmpty(reservationResponse.ErrorContent))
            {
                throw new InvalidOperationException($"Validating Reservations Failed, Status Code {reservationResponse.StatusCode} Error : {reservationResponse.ErrorContent}");
            }

            reservationValidationResult = reservationResponse.Body
                ?? throw new InvalidOperationException("Validating Reservations Failed, response body was null");
        }

        var providerStandardResults = await providerCoursesService.GetCoursesData(command.ProviderId);

        var uniqueCourseCodes = learnerDataResponse.Body.Select(r => r.TrainingCode).Distinct();
        var otjTrainingHours = await bulkCourseMetadataService.GetOtjTrainingHoursForBulkUploadAsync(uniqueCourseCodes);

        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords = learnerDataResponse.Body.Select((learner, index) =>
        {
            return new BulkUploadAddDraftApprenticeshipRequest
            {
                RowNumber = index + 1,
                Uln = learner.Uln.ToString(),
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                DateOfBirthAsString = learner.Dob.ToString("yyyy-MM-dd"),
                Email = learner.Email,
                CourseCode = learner.TrainingCode,
                StartDateAsString = learner.StartDate.ToString("yyyy-MM-dd"),
                EndDateAsString = learner.PlannedEndDate.ToString("yyyy-MM"),
                ProviderId = command.ProviderId,
                CostAsString = (learner.TrainingPrice + learner.EpaoPrice).ToString(),
                AgreementId = command.AgreementId,
                LegalEntityId = command.AccountLegalEntityId,
            };
        }).ToList();

        ValidateSelectMultipleLearnersApiRequest validateSelectMultipleLearnersApiRequest = new ValidateSelectMultipleLearnersApiRequest
        {
            CsvRecords = await courseTypesToCsvService.MapAndAddCourseTypeData(csvRecords),
            ProviderId = command.ProviderId,
            UserInfo = command.UserInfo,
            BulkReservationValidationResults = reservationValidationResult,
            ProviderStandardsData = providerStandardResults,
            OtjTrainingHours = otjTrainingHours
        };

        if (!validateSelectMultipleLearnersApiRequest.ProviderStandardsData.IsMainProvider)
        {
            validateSelectMultipleLearnersApiRequest.ProviderStandardsData.Standards = null;
        }

        var validationResponse = await apiClient.PostWithResponseCode<PostValidateSelectMultipleLearnersResponse>(
            new PostValidateSelectMultipleLearnersRequest(command.ProviderId, validateSelectMultipleLearnersApiRequest));

        if (!string.IsNullOrEmpty(validationResponse.ErrorContent))
        {
            throw new InvalidOperationException($"Validating Select Multiple Learners Failed, Status Code {validationResponse.StatusCode} Error : {validationResponse.ErrorContent}");
        }

        if (validationResponse.Body == null)
        {
            throw new InvalidOperationException("Validating Select Multiple Learners Failed, response body was null");
        }

        return new ValidateSelectMultipleLearnerRecordsQueryResult
        {
            ValidationErrors = validationResponse.Body.ValidationErrors ?? []
        };
    }

    private async Task ValidateExistingReservations(
        ValidateSelectMultipleLearnerRecordsQuery command,
        List<LearnerDataRecord> learners,
        List<SelectMultipleReservationAssignment> assignments)
    {
        var errors = new List<ErrorDetail>();
        if (assignments.Count != learners.Count ||
            assignments.Select(x => x.LearnerId).Distinct().Count() != learners.Count ||
            assignments.Any(x => !learners.Any(learner => learner.Id == x.LearnerId)))
        {
            errors.Add(new ErrorDetail("Assignments", "Every selected learner must have one reservation assignment."));
            ThrowDomainErrors(errors);
        }

        var existingAssignments = assignments
            .Select((assignment, index) => new { assignment, index })
            .Where(x => !x.assignment.UseNewReservation)
            .ToList();

        if (existingAssignments.Any(x => !x.assignment.ReservationId.HasValue) ||
            existingAssignments.Select(x => x.assignment.ReservationId).Distinct().Count() != existingAssignments.Count)
        {
            errors.Add(new ErrorDetail("Assignments", "Each existing reservation can only be assigned once."));
            ThrowDomainErrors(errors);
        }

        var reservations = await reservationApiClient.Get<List<GetAccountReservationsResponse>>(
            new GetAccountReservationsRequest(command.AccountId));

        var availableReservations = reservations
            .Where(x => x.AccountLegalEntityId == command.AccountLegalEntityId &&
                        x.Status == 0 &&
                        x.TransferSenderAccountId == null &&
                        (!x.ExpiryDate.HasValue || x.ExpiryDate.Value >= DateTime.UtcNow))
            .ToDictionary(x => x.Id);

        foreach (var existingAssignment in existingAssignments)
        {
            var field = $"Assignments[{existingAssignment.index}].ReservationId";
            if (!availableReservations.TryGetValue(existingAssignment.assignment.ReservationId.Value, out _))
            {
                errors.Add(new ErrorDetail(field, "The selected reservation is no longer available."));
                continue;
            }

            var learner = learners.Single(x => x.Id == existingAssignment.assignment.LearnerId);
            var result = await reservationApiClient.Get<ValidateReservationResponse>(
                new GetValidateReservationRequest(
                    existingAssignment.assignment.ReservationId.Value,
                    learner.TrainingCode,
                    learner.StartDate));

            foreach (var validationError in result.ValidationErrors ?? [])
            {
                errors.Add(new ErrorDetail(field, validationError.Reason));
            }
        }

        ThrowDomainErrors(errors);
    }

    private static void ThrowDomainErrors(List<ErrorDetail> errors)
    {
        if (errors.Count == 0)
        {
            return;
        }

        throw new DomainApimException(JsonConvert.SerializeObject(new ErrorResponse(errors)));
    }
}
