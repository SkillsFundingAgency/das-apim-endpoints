using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication
{
    public class GetApplicationViewQueryHandler(
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApplicationViewQuery, GetApplicationViewQueryResult>
    {
        public async Task<GetApplicationViewQueryResult> Handle(
            GetApplicationViewQuery request,
           CancellationToken cancellationToken)
        {
            var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));
            var qualificationTypesTask = candidateApiClient.Get<GetQualificationReferenceTypesApiResponse>(new GetQualificationReferenceTypesApiRequest());

            await Task.WhenAll(applicationTask, qualificationTypesTask);

            var application = applicationTask.Result;
            var qualificationTypes = qualificationTypesTask.Result;

            if (application == null) return null;

            var vacancy = await vacancyService.GetVacancy(application.VacancyReference) 
                          ?? await vacancyService.GetClosedVacancy(application.VacancyReference);

            if (vacancy == null) { return null; }

            var additionalQuestions = application
                .AdditionalQuestions
                .OrderBy(ord => ord.QuestionOrder)
                .ToList();

            var candidate = application.Candidate;
            var address = application.Candidate.Address;
            var trainingCourses = application.TrainingCourses;
            var jobs = application.WorkHistory?.Where(c => c.WorkHistoryType == WorkHistoryType.Job);
            var volunteeringExperiences = application.WorkHistory?.Where(c => c.WorkHistoryType == WorkHistoryType.WorkExperience);

            GetApplicationViewQueryResult.ApplicationQuestionsSection.Question additionalQuestion1 = null;
            GetApplicationViewQueryResult.ApplicationQuestionsSection.Question additionalQuestion2 = null;

            var additionalQuestion1Id =
                additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(0) != null
                    ? additionalQuestions[0].Id
                    : (Guid?)null;

            if (additionalQuestion1Id != null)
            {
                additionalQuestion1 = new GetApplicationViewQueryResult.ApplicationQuestionsSection.Question
                {
                    Answer = additionalQuestions[0].Answer,
                    QuestionLabel = additionalQuestions[0].QuestionText,
                };
            }

            var additionalQuestion2Id =
                additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null
                    ? additionalQuestions[1].Id
                    : (Guid?)null;

            if (additionalQuestion2Id != null)
            {
                additionalQuestion2 = new GetApplicationViewQueryResult.ApplicationQuestionsSection.Question
                {
                    Answer = additionalQuestions[1].Answer,
                    QuestionLabel = additionalQuestions[1].QuestionText,
                };
            }

            return new GetApplicationViewQueryResult
            {
                VacancyDetails = new GetApplicationViewQueryResult.VacancyDetailsSection
                {
                    Title = vacancy.Title,
                    EmployerName = vacancy.EmployerName
                },
                IsDisabilityConfident = application.DisabilityConfidenceStatus != "NotRequired",
                EducationHistory = new GetApplicationViewQueryResult.EducationHistorySection
                {
                    TrainingCourses = trainingCourses?.Select(x => (GetApplicationViewQueryResult.EducationHistorySection.TrainingCourse)x).ToList(),
                    Qualifications = application.Qualifications.Select(x => (GetApplicationViewQueryResult.EducationHistorySection.Qualification)x).OrderBy(fil => fil.QualificationOrder).ToList(),
                    QualificationTypes = qualificationTypes.QualificationReferences.Select(x => (GetApplicationViewQueryResult.EducationHistorySection.QualificationReference)x).ToList()
                },
                WorkHistory = new GetApplicationViewQueryResult.WorkHistorySection
                {
                    Jobs = jobs?.Select(x => (GetApplicationViewQueryResult.WorkHistorySection.Job)x).ToList(),
                    VolunteeringAndWorkExperiences = volunteeringExperiences?.Select(x => (GetApplicationViewQueryResult.WorkHistorySection.VolunteeringAndWorkExperience)x).ToList()
                },
                ApplicationQuestions = new GetApplicationViewQueryResult.ApplicationQuestionsSection
                {
                    AdditionalQuestion1 = additionalQuestion1,
                    AdditionalQuestion2 = additionalQuestion2,
                },
                InterviewAdjustments = new GetApplicationViewQueryResult.InterviewAdjustmentsSection
                {
                    InterviewAdjustmentsDescription = application.Support,
                },
                DisabilityConfidence = new GetApplicationViewQueryResult.DisabilityConfidenceSection
                {
                    ApplyUnderDisabilityConfidentScheme = application.ApplyUnderDisabilityConfidentScheme
                },
                CandidateDetails = new GetApplicationViewQueryResult.Candidate
                {
                    Id = candidate.Id,
                    GovUkIdentifier = candidate.GovUkIdentifier,
                    Email = candidate.Email,
                    FirstName = candidate.FirstName,
                    LastName = candidate.LastName,
                    MiddleName = candidate.MiddleNames,
                    PhoneNumber = candidate.PhoneNumber,
                    Address = address
                },
                EmploymentLocation = application.EmploymentLocation is not null
                    ? new GetApplicationViewQueryResult.EmploymentLocationSection
                    {
                        Id = application.EmploymentLocation.Id,
                        Addresses = application.EmploymentLocation.Addresses,
                        EmploymentLocationInformation = application.EmploymentLocation.EmploymentLocationInformation,
                        EmployerLocationOption = application.EmploymentLocation.EmployerLocationOption,
                    }
                    : null,
                AboutYou = new GetApplicationViewQueryResult.AboutYouSection
                {
                    SkillsAndStrengths = application.Strengths,
                    Support = application.Support
                },
                WhatIsYourInterest = new GetApplicationViewQueryResult.WhatIsYourInterestSection
                {
                    WhatIsYourInterest = application.WhatIsYourInterest
                },
                ApplicationStatus = application.Status.ToString(),
                WithdrawnDate = application.WithdrawnDate,
                MigrationDate = application.MigrationDate,
                ApprenticeshipType = vacancy.ApprenticeshipType,
            };
        }
    }
}
