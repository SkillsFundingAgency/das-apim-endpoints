using MediatR;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using SFA.DAS.VacanciesManage.Services;
using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;

public class CreateVacancyCommandHandler(IRecruitApiClient<RecruitApiV2Configuration> recruitApiV2Client,
    IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
    ICourseService courseService,
    ITrainingProviderService trainingProviderService,
    ISlaService slaService) : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
{
    public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        var dateTimeNow = DateTime.UtcNow;
        var accountLegalEntity = await accountLegalEntityPermissionService.GetAccountLegalEntity(request.AccountIdentifier,
            request.PostVacancyV2RequestData.AccountLegalEntityPublicHashedId);

        // if the account legal entity cannot be found or the account legal entity is not associated with the given Training Provider UKPRN, throw a security exception as the user should not have access to create a vacancy.
        if (accountLegalEntity == null)
        {
            throw new SecurityException();
        }

        // additional check to validate the given Training Provider UKPRN is valid.
        var trainingProvider = await trainingProviderService.GetProviderDetails((int)request.PostVacancyV2RequestData.TrainingProvider.Ukprn);
        if (trainingProvider == null)
        {
            throw new HttpRequestContentException("Training Provider UKPRN not valid", HttpStatusCode.NotFound);
        }

        // additional check to validate the given Training Provider is a Main or Employer Profile with Status not equal to "Not Currently Starting New Apprentices".
        if (!trainingProvider.IsTrainingProviderMainOrEmployerProfile)
        {
            throw new HttpRequestContentException("UKPRN of a training provider must be registered to deliver apprenticeship training", HttpStatusCode.Forbidden);
        }

        request.PostVacancyV2RequestData.LegalEntityName = accountLegalEntity.Name;

        request.PostVacancyV2RequestData.OwnerType = request.AccountIdentifier.AccountType == AccountType.Provider 
            ? OwnerType.Provider 
            : OwnerType.Employer;
            
        request.PostVacancyV2RequestData.AccountId = accountLegalEntity.AccountId;
        request.PostVacancyV2RequestData.AccountLegalEntityId = accountLegalEntity.AccountLegalEntityId;

        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        var course = standards.Standards.FirstOrDefault(c =>
            c.LarsCode.ToString() == request.PostVacancyV2RequestData.ProgrammeId);
        
        request.PostVacancyV2RequestData.ApprenticeshipType = "Standard";
            
        if (course is { ApprenticeshipType: ApprenticeshipType.FoundationApprenticeship })
        {
            request.PostVacancyV2RequestData.Qualifications = [];
            request.PostVacancyV2RequestData.Skills = [];
            request.PostVacancyV2RequestData.ApprenticeshipType = "Foundation";
        }

        if (course != null && ((course.LastDateStarts != null && course.LastDateStarts < request.PostVacancyV2RequestData.StartDate)
                               || (course.EffectiveTo !=null && course.EffectiveTo < request.PostVacancyV2RequestData.StartDate)))
        {
            var dateToDisplay = course.LastDateStarts ?? course.EffectiveTo.Value;
                    
            var message = $"Start date must be on or before {dateToDisplay} as this is the last day for new starters for the training course you have selected. If you don't want to change the start date, you can change the training course";
            throw new HttpRequestContentException(
                $"Response status code does not indicate success: {(int) HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})",
                HttpStatusCode.BadRequest,
                message);
        }
        var postValidateVacancyRequest = new PostVacancyV2Request(request.PostVacancyV2RequestData);

        var requiresEmployerApproval = await CheckEmployerApprovalNeeded(request);

        if (requiresEmployerApproval)
        {
            request.PostVacancyV2RequestData.Status = nameof(VacancyStatus.Review);
        }
        else
        {
            request.PostVacancyV2RequestData.Status = nameof(VacancyStatus.Submitted);
            request.PostVacancyV2RequestData.SubmittedDate = dateTimeNow;
        }

        var result = await recruitApiV2Client.PostWithResponseCode<PostVacancyResponse>(postValidateVacancyRequest);
            
        HandleHttpResponseError(result);

        if (result is { StatusCode: HttpStatusCode.Created } && requiresEmployerApproval)
        {
            var slaDeadline = await slaService.GetSlaDeadlineAsync(dateTimeNow);
            var vacancyReview = new PutVacancyReviewRequest.PutVacancyReviewRequestData
            {
                VacancyReference = result.Body.VacancyReference,
                VacancyTitle = request.PostVacancyV2RequestData.Title,
                CreatedDate = dateTimeNow,
                Status = ReviewStatus.New,
                VacancySnapshot = JsonConvert.SerializeObject(request.PostVacancyV2RequestData),
                SubmittedByUserEmail = request.PostVacancyV2RequestData?.Contact?.Email,
                SubmissionCount = 1,
                SlaDeadLine = slaDeadline,
                UpdatedFieldIdentifiers = [],
                DismissedAutomatedQaOutcomeIndicators = [],
            };
            var response = await recruitApiV2Client.PutWithResponseCode<PutVacancyReviewResponse>(new PutVacancyReviewRequest(Guid.NewGuid(), vacancyReview));
            response.EnsureSuccessStatusCode();
        }

        return new CreateVacancyCommandResponse(result.Body.VacancyReference.ToString());
    }

    private static void HandleHttpResponseError<T>(ApiResponse<T> result)
    {
        if ((int) result.StatusCode >= 200 && (int) result.StatusCode <= 299) return;
        if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);    
        }

        throw new Exception(
            $"Response status code does not indicate success: {(int) result.StatusCode} ({result.StatusCode})");
    }

    private async Task<bool> CheckEmployerApprovalNeeded(CreateVacancyCommand request)
    {
        if (request.PostVacancyV2RequestData.OwnerType != OwnerType.Provider)
            return false;

        bool hasPermission = await accountLegalEntityPermissionService.HasProviderGotEmployersPermissionAsync(
            request.PostVacancyV2RequestData.TrainingProvider.Ukprn,
            request.PostVacancyV2RequestData.AccountId,
            [Operation.RecruitmentRequiresReview]); 
            
        return !hasPermission;
    }
}