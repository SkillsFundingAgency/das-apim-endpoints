using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using SFA.DAS.VacanciesManage.Services;
using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using HttpRequestContentException = SFA.DAS.Apim.Shared.Infrastructure.HttpRequestContentException;
using Operation = SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships.Operation;
using OwnerType = SFA.DAS.Recruit.Contracts.ApiResponses.OwnerType;
using PutVacancyReviewRequest = SFA.DAS.Recruit.Contracts.ApiResponses.PutVacancyReviewRequest;
using ReviewStatus = SFA.DAS.Recruit.Contracts.ApiResponses.ReviewStatus;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;

public class CreateVacancyCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
    ICourseService courseService,
    ITrainingProviderService trainingProviderService,
    ISlaService slaService) : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
{
    public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        var dateTimeNow = DateTime.UtcNow;
        var accountLegalEntity = await accountLegalEntityPermissionService.GetAccountLegalEntity(request.AccountIdentifier,
            request.PostVacancyRequest.AccountLegalEntityPublicHashedId);

        // if the account legal entity cannot be found or the account legal entity is not associated with the given Training Provider UKPRN, throw a security exception as the user should not have access to create a vacancy.
        if (accountLegalEntity == null)
        {
            throw new SecurityException();
        }
        if (request.PostVacancyRequest.TrainingProvider.Ukprn is null)
        {
            throw new HttpRequestContentException("Training Provider UKPRN not valid", HttpStatusCode.NotFound);
        }

        // additional check to validate the given Training Provider UKPRN is valid.
        var trainingProvider = await trainingProviderService.GetProviderDetails((int)request.PostVacancyRequest.TrainingProvider.Ukprn!);
        if (trainingProvider == null)
        {
            throw new HttpRequestContentException("Training Provider UKPRN not valid", HttpStatusCode.NotFound);
        }

        // additional check to validate the given Training Provider is a Main or Employer Profile with Status not equal to "Not Currently Starting New Apprentices".
        if (!trainingProvider.IsTrainingProviderMainOrEmployerProfile)
        {
            throw new HttpRequestContentException("UKPRN of a training provider must be registered to deliver apprenticeship training", HttpStatusCode.Forbidden);
        }

        request.PostVacancyRequest.LegalEntityName = accountLegalEntity.Name;

        request.PostVacancyRequest.OwnerType = request.AccountIdentifier.AccountType == AccountType.Provider
            ? OwnerType.Provider
            : OwnerType.Employer;

        request.PostVacancyRequest.AccountId = accountLegalEntity.AccountId;
        request.PostVacancyRequest.AccountLegalEntityId = accountLegalEntity.AccountLegalEntityId;

        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        var course = standards.Standards.FirstOrDefault(c =>
            c.LarsCode.ToString() == request.PostVacancyRequest.ProgrammeId);

        request.PostVacancyRequest.ApprenticeshipType = ApprenticeshipTypes.Standard;

        if (course is { ApprenticeshipType: LearningType.FoundationApprenticeship })
        {
            request.PostVacancyRequest.Qualifications = [];
            request.PostVacancyRequest.Skills = [];
            request.PostVacancyRequest.ApprenticeshipType = ApprenticeshipTypes.Foundation;
        }

        if (course != null && ((course.LastDateStarts != null && course.LastDateStarts < request.PostVacancyRequest.StartDate)
                               || (course.EffectiveTo != null && course.EffectiveTo < request.PostVacancyRequest.StartDate)))
        {
            var dateToDisplay = course.LastDateStarts ?? course.EffectiveTo.Value;

            var message = $"Start date must be on or before {dateToDisplay} as this is the last day for new starters for the training course you have selected. If you don't want to change the start date, you can change the training course";
            throw new HttpRequestContentException(
                $"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})",
                HttpStatusCode.BadRequest,
                message);
        }

        var requiresEmployerApproval = await CheckEmployerApprovalNeeded(request);

        if (requiresEmployerApproval)
        {
            request.PostVacancyRequest.Status = VacancyStatus.Review;
        }
        else
        {
            request.PostVacancyRequest.Status = VacancyStatus.Submitted;
            request.PostVacancyRequest.SubmittedDate = dateTimeNow;
        }

        var result = await recruitApiClient.PostWithResponseCode<Vacancy>(new PostVacanciesApiRequest(request.PostVacancyRequest)
        {
            RuleSet = VacancyRuleSet.All,
            ValidateOnly = false,
        });

        HandleHttpResponseError(result);

        if (result is not { StatusCode: HttpStatusCode.Created } || !requiresEmployerApproval)
            return new CreateVacancyCommandResponse(result.Body.VacancyReference.ToString());

        var slaDeadline = await slaService.GetSlaDeadlineAsync(dateTimeNow);
        var vacancyReview = new PutVacancyreviewsByIdApiRequest
        {
            Id = Guid.NewGuid(),
            Data = new PutVacancyReviewRequest
            {
                VacancyReference = result.Body.VacancyReference.ToString(),
                VacancyTitle = request.PostVacancyRequest.Title,
                CreatedDate = dateTimeNow,
                Status = ReviewStatus.New,
                VacancySnapshot = JsonSerializer.Serialize(request.PostVacancyRequest, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                }),
                SubmittedByUserEmail = request.PostVacancyRequest?.Contact.Email,
                SubmissionCount = 1,
                SlaDeadLine = slaDeadline,
                UpdatedFieldIdentifiers = [],
                DismissedAutomatedQaOutcomeIndicators = [],
            }
        };

        var reviewResponse = await recruitApiClient.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(vacancyReview);
        reviewResponse.EnsureSuccessStatusCode();

        return new CreateVacancyCommandResponse(result.Body.VacancyReference.ToString());
    }

    private static void HandleHttpResponseError<T>(Apim.Shared.Models.ApiResponse<T> result)
    {
        if ((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299) return;
        if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
        }

        throw new Exception(
            $"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})");
    }

    private async Task<bool> CheckEmployerApprovalNeeded(CreateVacancyCommand request)
    {
        if (request.PostVacancyRequest.OwnerType != OwnerType.Provider)
            return false;

        if (request.PostVacancyRequest.TrainingProvider.Ukprn == null)
            return false;

        if (request.PostVacancyRequest.AccountId is null)
            return false;

        bool hasPermission = await accountLegalEntityPermissionService.HasProviderGotEmployersPermissionAsync(
            (long)request.PostVacancyRequest.TrainingProvider.Ukprn,
            (long)request.PostVacancyRequest.AccountId,
            [Operation.RecruitmentRequiresReview]);

        return !hasPermission;
    }
}