using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using SFA.DAS.VacanciesManage.Services;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EmployerNameOption = SFA.DAS.Recruit.Contracts.ApiResponses.EmployerNameOption;
using HttpRequestContentException = SFA.DAS.Apim.Shared.Infrastructure.HttpRequestContentException;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;
using Operation = SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships.Operation;
using OwnerType = SFA.DAS.Recruit.Contracts.ApiResponses.OwnerType;
using PutVacancyReviewRequest = SFA.DAS.Recruit.Contracts.ApiResponses.PutVacancyReviewRequest;
using ReviewStatus = SFA.DAS.Recruit.Contracts.ApiResponses.ReviewStatus;
using Vacancy = SFA.DAS.Recruit.Contracts.ApiResponses.Vacancy;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;

public class CreateVacancyCommandHandler(
    IRecruitApiClient recruitApiClient,
    IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
    ICourseService courseService,
    ISlaService slaService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
    : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
{

    public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        var dateTimeNow = DateTime.UtcNow;
        var vacancy = request.PostVacancyRequest;
        
        ValidateUkprn(vacancy);

        //additional check to validate the given Training Provider UKPRN is valid.
        await ValidatedTrainingProvider(vacancy);

        var accountLegalEntity = await GetValidatedAccountLegalEntity(request);

        EnrichVacancy(vacancy, request, accountLegalEntity);

        var course = await GetCourse(vacancy.ProgrammeId, cancellationToken);

        ApplyCourseRules(vacancy, course);
       
        ValidateCourseStartDate(vacancy, course);

        var requiresEmployerApproval = await RequiresEmployerApproval(vacancy);

        ApplyVacancyStatus(vacancy, requiresEmployerApproval, dateTimeNow);

        vacancy.Id = request.Id;

        var result = await CreateVacancy(vacancy, request.IsSandbox);

        HandleHttpResponseError(result);

        if (!request.IsSandbox && requiresEmployerApproval)
        {
            await CreateVacancyReview(vacancy, result.Body.VacancyReference.ToString(), dateTimeNow);
        }
        
        return new CreateVacancyCommandResponse(result.Body.VacancyReference.ToString());
    }

    private async Task CreateVacancyReview(PostVacancyRequest vacancy, string vacancyReference, DateTime createdDate)
    {
        var slaDeadline =
            await slaService.GetSlaDeadlineAsync(createdDate);

        var reviewRequest = new PutVacancyreviewsByIdApiRequest
        {
            Id = Guid.NewGuid(),
            Data = new PutVacancyReviewRequest
            {
                VacancyReference = vacancyReference,
                VacancyTitle = vacancy.Title,
                CreatedDate = createdDate,
                Status = ReviewStatus.New,
                VacancySnapshot = JsonSerializer.Serialize(vacancy, options: new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                }),
                SubmittedByUserEmail = vacancy.Contact.Email,
                SubmissionCount = 1,
                SlaDeadLine = slaDeadline,
                UpdatedFieldIdentifiers = [],
                DismissedAutomatedQaOutcomeIndicators = []
            }
        };

        var response = await recruitApiClient.PutWithResponseCode<PutVacancyReviewRequest, VacancyReview>(reviewRequest);
        response.EnsureSuccessStatusCode();
    }

    private static void ValidateUkprn(PostVacancyRequest vacancy)
    {
        if (vacancy.TrainingProvider.Ukprn is null)
        {
            throw new HttpRequestContentException("Training Provider UKPRN not valid", HttpStatusCode.NotFound);
        }
    }

    private async Task<AccountLegalEntityItem> GetValidatedAccountLegalEntity(CreateVacancyCommand request)
    {
        var accountLegalEntity =
            await accountLegalEntityPermissionService.GetAccountLegalEntity(
                request.AccountIdentifier,
                request.PostVacancyRequest.AccountLegalEntityPublicHashedId);

        return accountLegalEntity ?? throw new UnauthorizedAccessException("Account legal entity not accessible.");
    }

    private async Task ValidatedTrainingProvider(PostVacancyRequest vacancy)
    {
        var provider =
            await roatpCourseManagementApiClient.Get<GetProvidersListItem>(
                new GetProvidersRequest((int)vacancy.TrainingProvider.Ukprn!));

        if (provider is null)
        {
            throw new HttpRequestContentException("Training Provider UKPRN not valid", HttpStatusCode.NotFound);
        }

        if (provider.ProviderTypeId == (int)ProviderType.Supporting)
        {
            throw new HttpRequestContentException("UKPRN of a training provider must be registered to deliver apprenticeship training", HttpStatusCode.Forbidden);
        }
    }

    private static void EnrichVacancy(PostVacancyRequest vacancy, CreateVacancyCommand request, AccountLegalEntityItem accountLegalEntity)
    {
        vacancy.LegalEntityName = accountLegalEntity.Name;

        vacancy.OwnerType =
            request.AccountIdentifier.AccountType == AccountType.Provider
                ? OwnerType.Provider
                : OwnerType.Employer;

        vacancy.AccountId = accountLegalEntity.AccountId;
        vacancy.AccountLegalEntityId = accountLegalEntity.AccountLegalEntityId;

        if (vacancy.EmployerNameOption == EmployerNameOption.RegisteredName)
        {
            vacancy.EmployerName = accountLegalEntity.Name;
        }
    }

    private async Task<GetStandardsListItem?> GetCourse(string programmeId, CancellationToken cancellationToken)
    {
        var standards =
            await courseService.GetActiveStandards<GetStandardsListResponse>(
                nameof(GetStandardsListResponse));

        return standards.Standards.FirstOrDefault(x =>
            x.LarsCode.ToString() == programmeId);
    }

    private static void ApplyCourseRules(PostVacancyRequest vacancy, GetStandardsListItem? course)
    {
        vacancy.ApprenticeshipType = ApprenticeshipTypes.Standard;

        if (course is
            {
                ApprenticeshipType: LearningType.FoundationApprenticeship
            })
        {
            vacancy.Qualifications = [];
            vacancy.Skills = [];
            vacancy.ApprenticeshipType = ApprenticeshipTypes.Foundation;
        }
    }

    private static void ValidateCourseStartDate(PostVacancyRequest vacancy, GetStandardsListItem? course)
    {
        if (course is null)
        {
            return;
        }

        var exceedsLastDateStarts = course.LastDateStarts is not null &&
                                    course.LastDateStarts < vacancy.StartDate;

        var exceedsEffectiveTo = course.EffectiveTo is not null &&
                                 course.EffectiveTo < vacancy.StartDate;

        if (!exceedsLastDateStarts && !exceedsEffectiveTo)
        {
            return;
        }

        var lastAllowedDate =
            course.LastDateStarts ?? course.EffectiveTo!.Value;

        var message = $"Start date must be on or before {lastAllowedDate:d} as this is the last day for new starters for the training course you have selected. If you don't want to change the start date, you can change the training course";
        throw new HttpRequestContentException(
            $"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})",
            HttpStatusCode.BadRequest,
            message);
    }

    private static void HandleHttpResponseError<T>(ApiResponse<T> result)
    {
        if ((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299) return;
        if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
        }

        throw new Exception(
            $"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})");
    }

    private async Task<bool> RequiresEmployerApproval(PostVacancyRequest vacancy)
    {
        if (vacancy.OwnerType != OwnerType.Provider)
        {
            return false;
        }

        if (vacancy.TrainingProvider.Ukprn is null)
        {
            return false;
        }

        if (vacancy.AccountId is null)
        {
            return false;
        }

        var hasPermission =
            await accountLegalEntityPermissionService
                .HasProviderGotEmployersPermissionAsync(
                    (long)vacancy.TrainingProvider.Ukprn,
                    (long)vacancy.AccountId,
                    [Operation.RecruitmentRequiresReview]);

        return !hasPermission;
    }

    private static void ApplyVacancyStatus(PostVacancyRequest vacancy, bool requiresEmployerApproval, DateTime now)
    {
        if (requiresEmployerApproval)
        {
            vacancy.Status = VacancyStatus.Review;
            return;
        }

        vacancy.Status = VacancyStatus.Submitted;
        vacancy.SubmittedDate = now;
    }

    private async Task<ApiResponse<Vacancy>> CreateVacancy(PostVacancyRequest vacancy, bool isSandbox)
    {
        var response =
            await recruitApiClient.PostWithResponseCode<Vacancy>(
                new PostVacanciesApiRequest(vacancy)
                {
                    RuleSet = VacancyRuleSet.All,
                    ValidateOnly = isSandbox
                });

        return response;
    }
}