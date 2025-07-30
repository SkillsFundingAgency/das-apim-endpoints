using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

public class GetIndexQueryHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService)
    : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{
    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        if (application == null) return null;

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference)
                      ?? await vacancyService.GetClosedVacancy(application.VacancyReference);

        if (vacancy is null) return null;

        GetApplicationApiResponse previousApplication = null;
        GetApprenticeshipVacancyQueryResult.Vacancy previousVacancy = null;
        if (application.PreviousAnswersSourceId.HasValue)
        {
            previousApplication = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, application.PreviousAnswersSourceId.Value, false));
            if (previousApplication != null)
            {
                var previousVacancyFromApi = await vacancyService.GetVacancy(previousApplication.VacancyReference);
                if (previousVacancyFromApi != null)
                {
                    previousVacancy = GetApprenticeshipVacancyQueryResult.Vacancy.FromIVacancy(previousVacancyFromApi);
                }
            }
        }

        var additionalQuestions = application
            .AdditionalQuestions
            .OrderBy(ord => ord.QuestionOrder)
            .ToList();

        return new GetIndexQueryResult
        {
            VacancyReference = vacancy.VacancyReference,
            VacancyTitle = vacancy.Title,
            ApprenticeshipType = vacancy.ApprenticeshipType,
            Address = vacancy.Address,
            OtherAddresses = vacancy.OtherAddresses,
            EmployerLocationOption = vacancy.EmployerLocationOption,
            EmployerName = vacancy.EmployerName,
            ClosingDate = vacancy.ClosingDate,
            ClosedDate = vacancy.ClosedDate,
            IsMigrated = application.MigrationDate.HasValue,
            IsDisabilityConfident = vacancy.IsDisabilityConfident,
            IsApplicationComplete = application.ApplicationAllSectionStatus.Equals("Completed", StringComparison.CurrentCultureIgnoreCase),
            EducationHistory = new GetIndexQueryResult.EducationHistorySection
            {
                Qualifications = application.QualificationsStatus,
                TrainingCourses = application.TrainingCoursesStatus,
            },
            EmploymentLocation = application.EmploymentLocation is not null
                ? new GetIndexQueryResult.EmploymentLocationSection
                {
                    Id = application.EmploymentLocation.Id,
                    Addresses = application.EmploymentLocation.Addresses,
                    EmploymentLocationInformation = application.EmploymentLocation.EmploymentLocationInformation,
                    EmployerLocationOption = application.EmploymentLocation.EmployerLocationOption,
                    EmploymentLocationStatus = application.EmploymentLocationStatus
                } : null,
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
                AdditionalQuestion1Label = GetAdditionalQuestion(additionalQuestions, 1)?.QuestionText,
                AdditionalQuestion1Id = GetAdditionalQuestion(additionalQuestions, 1)?.Id,
                AdditionalQuestion2 = application.AdditionalQuestion2Status,
                AdditionalQuestion2Label = GetAdditionalQuestion(additionalQuestions, 2)?.QuestionText,
                AdditionalQuestion2Id = GetAdditionalQuestion(additionalQuestions, 2)?.Id,
            },
            InterviewAdjustments = new GetIndexQueryResult.InterviewAdjustmentsSection
            {
                RequestAdjustments = application.InterviewAdjustmentsStatus
            },
            DisabilityConfidence = new GetIndexQueryResult.DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfident = application.DisabilityConfidenceStatus,
            },
            PreviousApplication = previousVacancy == null ? null : new GetIndexQueryResult.PreviousApplicationDetails
            {
                EmployerName = previousVacancy.EmployerName,
                SubmissionDate = previousApplication.SubmittedDate ?? DateTime.UtcNow,
                VacancyTitle = previousVacancy.Title
            }
        };
    }

    private static Question GetAdditionalQuestion(List<Question> additionalQuestions, int questionNumber)
    {
        var additionalQuestion = additionalQuestions is { Count: > 0 } && additionalQuestions.FirstOrDefault(c => c.QuestionOrder == questionNumber) != null
            ? additionalQuestions.FirstOrDefault(c => c.QuestionOrder == questionNumber)!
            : null;

        if (additionalQuestion == null && additionalQuestions.Count > 0 && additionalQuestions.TrueForAll(c => c.QuestionOrder == null))
        {
            if (additionalQuestions is { Count: >= 2 })
            {
                switch (questionNumber)
                {
                    case 1:
                        return additionalQuestions.FirstOrDefault();
                    case 2:
                        return additionalQuestions.LastOrDefault();
                }
            }

            if (additionalQuestions is { Count: 1 } && questionNumber == 1)
            {
                return additionalQuestions.FirstOrDefault();
            }
        }


        return additionalQuestion;
    }
}