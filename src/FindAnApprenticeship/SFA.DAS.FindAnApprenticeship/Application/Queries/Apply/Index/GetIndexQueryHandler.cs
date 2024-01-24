using System;
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

        //todo: change this to a GET for application from PUT
        var putData = new PutApplicationApiRequest.PutApplicationApiRequestData
        {
            CandidateId = request.CandidateId
        };
        var vacancyReference = request.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
        var putRequest = new PutApplicationApiRequest(vacancyReference, putData);

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
                Qualifications = applicationResult.Body.QualificationsStatus,
                TrainingCourses = applicationResult.Body.TrainingCoursesStatus,
            },
            WorkHistory = new GetIndexQueryResult.WorkHistorySection
            {
                Jobs = applicationResult.Body.JobsStatus,
                VolunteeringAndWorkExperience = applicationResult.Body.WorkExperienceStatus,
            },
            ApplicationQuestions = new GetIndexQueryResult.ApplicationQuestionsSection
            {
                SkillsAndStrengths = applicationResult.Body.SkillsAndStrengthStatus,
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