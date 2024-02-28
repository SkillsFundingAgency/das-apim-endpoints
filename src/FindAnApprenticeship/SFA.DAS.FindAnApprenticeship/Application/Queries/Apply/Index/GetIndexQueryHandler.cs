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

        var additionalQuestions = vacancy.AdditionalQuestions.ToList();

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
                AdditionalQuestion1Label = additionalQuestions[0].QuestionId,
                AdditionalQuestion1Id = additionalQuestions[0].Id,
                AdditionalQuestion2 = application.AdditionalQuestion2Status,
                AdditionalQuestion2Label = additionalQuestions[1].QuestionId,
                AdditionalQuestion2Id = additionalQuestions[1].Id
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