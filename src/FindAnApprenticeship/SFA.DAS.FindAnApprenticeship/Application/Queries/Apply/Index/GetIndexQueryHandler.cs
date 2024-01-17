using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery,GetIndexQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;

    public GetIndexQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
    }

    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));

        if (result is null) return null;

        return new GetIndexQueryResult
        {
            VacancyTitle = result.Title,
            EmployerName = result.EmployerName,
            ClosingDate = result.ClosingDate,
            IsDisabilityConfident = result.IsDisabilityConfident,
            EducationHistory = new GetIndexQueryResult.EducationHistorySection
            {
                Qualifications = Constants.SectionStatus.NotStarted,
                TrainingCourses = Constants.SectionStatus.NotStarted
            },
            WorkHistory = new GetIndexQueryResult.WorkHistorySection
            {
                Jobs = Constants.SectionStatus.NotStarted,
                VolunteeringAndWorkExperience = Constants.SectionStatus.NotStarted
            },
            ApplicationQuestions = new GetIndexQueryResult.ApplicationQuestionsSection
            {
                SkillsAndStrengths = Constants.SectionStatus.NotStarted,
                WhatInterestsYou = Constants.SectionStatus.NotStarted,
                AdditionalQuestion1 = Constants.SectionStatus.NotStarted,
                AdditionalQuestion1Label = result.AdditionalQuestion1,
                AdditionalQuestion2 = Constants.SectionStatus.NotStarted,
                AdditionalQuestion2Label = result.AdditionalQuestion2
            },
            InterviewAdjustments = new GetIndexQueryResult.InterviewAdjustmentsSection
            {
                RequestAdjustments = Constants.SectionStatus.NotStarted
            },
            DisabilityConfidence = new GetIndexQueryResult.DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfident = Constants.SectionStatus.NotStarted
            }
        };
    }
}