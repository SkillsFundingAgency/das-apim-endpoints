using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery,GetIndexQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetIndexQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));

        var putData = new PatchApplicationApiRequest.PutApplicationApiRequestData
        {
            Email = request.ApplicantEmailAddress
        };
        var putRequest = new PatchApplicationApiRequest(request.VacancyReference, putData);

        var applicationResult = await _candidateApiClient.PutWithResponseCode<PutApplicationApiResponse>(putRequest);

        applicationResult.EnsureSuccessStatusCode();

        if (result is null) return null;
        if(applicationResult is null) return null;

        return new GetIndexQueryResult
        {
            VacancyTitle = result.Title,
            EmployerName = result.EmployerName,
            ClosingDate = result.ClosingDate,
            IsDisabilityConfident = result.IsDisabilityConfident,
            EducationHistory = new GetIndexQueryResult.EducationHistorySection
            {
                Qualifications = applicationResult.Body.QualificationStatus,
                TrainingCourses = applicationResult.Body.TrainingCourseStatus,
            },
            WorkHistory = new GetIndexQueryResult.WorkHistorySection
            {
                Jobs = applicationResult.Body.JobStatus,
                VolunteeringAndWorkExperience = applicationResult.Body.WorkExperienceStatus,
            },
            ApplicationQuestions = new GetIndexQueryResult.ApplicationQuestionsSection
            {
                SkillsAndStrengths = applicationResult.Body.SkillsAndStrengthsStatus,
                WhatInterestsYou = applicationResult.Body.InterestsStatus,
                AdditionalQuestion1 = applicationResult.Body.AdditionalQuestion1Status,
                AdditionalQuestion1Label = result.AdditionalQuestion1,
                AdditionalQuestion2 = applicationResult.Body.AdditionalQuestion2Status,
                AdditionalQuestion2Label = result.AdditionalQuestion2
            },
            InterviewAdjustments = new GetIndexQueryResult.InterviewAdjustmentsSection
            {
                RequestAdjustments = applicationResult.Body.InterviewAdjustmentsStatus
            },
            DisabilityConfidence = new GetIndexQueryResult.DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfident = applicationResult.Body.DisabilityConfidenceStatus,
            }
        };
    }
}