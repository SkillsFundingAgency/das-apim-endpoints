using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
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
        var application = await _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId));
        if (application == null) return null;

        var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(application.VacancyReference));
        if(vacancy == null) return null;

        var additionalQuestions = application.AdditionalQuestions.ToList();

        return new GetIndexQueryResult
        {
            VacancyReference = vacancy.VacancyReference,
            VacancyTitle = vacancy.Title,
            EmployerName = vacancy.EmployerName,
            ClosingDate = vacancy.ClosingDate,
            IsDisabilityConfident = vacancy.IsDisabilityConfident,
            EducationHistory = new GetIndexQueryResult.EducationHistorySection
            {
                Qualifications = application.QualificationsStatus,
                TrainingCourses = application.TrainingCoursesStatus,
            },
            WorkHistory = new GetIndexQueryResult.WorkHistorySection
            {
                Jobs = application.JobsStatus,
                VolunteeringAndWorkExperience = application.WorkExperienceStatus,
            },
            ApplicationQuestions = new GetIndexQueryResult.ApplicationQuestionsSection
            {
                SkillsAndStrengths = application.SkillsAndStrengthStatus,
                WhatInterestsYou = application.InterestsStatus,
                AdditionalQuestion1 = application.AdditionalQuestion1Status,
                AdditionalQuestion1Label = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(0) != null ? additionalQuestions[0].QuestionText : null,
                AdditionalQuestion1Id = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(0) != null ? additionalQuestions[0].Id : null,
                AdditionalQuestion2 = application.AdditionalQuestion2Status,
                AdditionalQuestion2Label = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null ? additionalQuestions[1].QuestionText : null,
                AdditionalQuestion2Id = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null ? additionalQuestions[1].Id : null
            },
            InterviewAdjustments = new GetIndexQueryResult.InterviewAdjustmentsSection
            {
                RequestAdjustments = application.InterviewAdjustmentsStatus
            },
            DisabilityConfidence = new GetIndexQueryResult.DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfident = application.DisabilityConfidenceStatus,
            }
        };
    }
}