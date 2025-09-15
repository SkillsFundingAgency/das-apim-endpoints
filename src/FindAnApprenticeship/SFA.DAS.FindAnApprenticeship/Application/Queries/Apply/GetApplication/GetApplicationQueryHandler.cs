using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication
{
    public class GetApplicationQueryHandler(
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApplicationQuery, GetApplicationQueryResult>
    {
        public async Task<GetApplicationQueryResult> Handle(
        GetApplicationQuery request,
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

            if (vacancy is null) return null;

            var additionalQuestions = application
                .AdditionalQuestions
                .OrderBy(ord => ord.QuestionOrder)
                .ToList();

            var candidate = application.Candidate;
            var address = application.Candidate.Address;
            var trainingCourses = application.TrainingCourses;
            var jobs = application.WorkHistory?.Where(c=>c.WorkHistoryType == WorkHistoryType.Job);
            var volunteeringExperiences = application.WorkHistory?.Where(c=>c.WorkHistoryType == WorkHistoryType.WorkExperience);

            GetApplicationQueryResult.ApplicationQuestionsSection.Question additionalQuestion1 = null;
            GetApplicationQueryResult.ApplicationQuestionsSection.Question additionalQuestion2 = null;

            var additionalQuestion1Id =
                additionalQuestions is {Count: > 0} && additionalQuestions.ElementAtOrDefault(0) != null
                    ? additionalQuestions[0].Id
                    : (Guid?) null;

            if (additionalQuestion1Id != null)
            {
                additionalQuestion1 = new GetApplicationQueryResult.ApplicationQuestionsSection.Question
                {
                    Id = additionalQuestions[0].Id,
                    Answer = additionalQuestions[0].Answer,
                    QuestionLabel = additionalQuestions[0].QuestionText,
                    Status = application.AdditionalQuestion1Status
                };
            }

            var additionalQuestion2Id =
                additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null
                    ? additionalQuestions[1].Id
                    : (Guid?)null;

            if (additionalQuestion2Id != null)
            {
                additionalQuestion2 = new GetApplicationQueryResult.ApplicationQuestionsSection.Question
                {
                    Id = additionalQuestions[1].Id,
                    Answer = additionalQuestions[1].Answer,
                    QuestionLabel = additionalQuestions[1].QuestionText,
                    Status = application.AdditionalQuestion1Status
                };
            }

            return new GetApplicationQueryResult
            {
                ClosingDate = vacancy.ClosingDate,
                ClosedDate = vacancy.ClosedDate,
                EmployerName = vacancy.EmployerName,
                VacancyTitle = vacancy.Title,
                IsDisabilityConfident = application.DisabilityConfidenceStatus != "NotRequired",
                ApprenticeshipType = vacancy.ApprenticeshipType,
                EducationHistory = new GetApplicationQueryResult.EducationHistorySection
                {
                    QualificationsStatus = application.QualificationsStatus,
                    TrainingCoursesStatus = application.TrainingCoursesStatus,
                    TrainingCourses = trainingCourses?.Select(x => (GetApplicationQueryResult.EducationHistorySection.TrainingCourse)x).ToList(),
                    Qualifications = application.Qualifications.Select(x => (GetApplicationQueryResult.EducationHistorySection.Qualification)x).OrderBy(fil => fil.QualificationOrder).ToList(),
                    QualificationTypes = qualificationTypes.QualificationReferences.Select(x => (GetApplicationQueryResult.EducationHistorySection.QualificationReference)x).ToList()
                },
                WorkHistory = new GetApplicationQueryResult.WorkHistorySection
                {
                    JobsStatus = application.JobsStatus,
                    VolunteeringAndWorkExperienceStatus = application.WorkExperienceStatus,
                    Jobs = jobs?.Select(x => (GetApplicationQueryResult.WorkHistorySection.Job)x).ToList(),
                    VolunteeringAndWorkExperiences = volunteeringExperiences?.Select(x => (GetApplicationQueryResult.WorkHistorySection.VolunteeringAndWorkExperience)x).ToList()
                },
                ApplicationQuestions = new GetApplicationQueryResult.ApplicationQuestionsSection
                {
                    SkillsAndStrengthsStatus = application.SkillsAndStrengthStatus,
                    WhatInterestsYouStatus = application.InterestsStatus,
                    AdditionalQuestion1 = additionalQuestion1,
                    AdditionalQuestion2 = additionalQuestion2,
                },
                InterviewAdjustments = new GetApplicationQueryResult.InterviewAdjustmentsSection
                {
                    RequestAdjustmentsStatus = application.InterviewAdjustmentsStatus,
                    InterviewAdjustmentsDescription = application.Support,
                },
                DisabilityConfidence = new GetApplicationQueryResult.DisabilityConfidenceSection
                {
                    InterviewUnderDisabilityConfidentStatus = application.DisabilityConfidenceStatus,
                    ApplyUnderDisabilityConfidentScheme = application.ApplyUnderDisabilityConfidentScheme
                },
                CandidateDetails = new GetApplicationQueryResult.Candidate
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
                    ? new GetApplicationQueryResult.EmploymentLocationSection
                    {
                        Id = application.EmploymentLocation.Id,
                        EmploymentLocationStatus = application.EmploymentLocationStatus,
                        Addresses = application.EmploymentLocation.Addresses,
                        EmploymentLocationInformation = application.EmploymentLocation.EmploymentLocationInformation,
                        EmployerLocationOption = application.EmploymentLocation.EmployerLocationOption,
                    }
                    : null,
                AboutYou = new GetApplicationQueryResult.AboutYouSection
                {
                    SkillsAndStrengths = application.Strengths,
                    Support = application.Support,
                },
                WhatIsYourInterest = new GetApplicationQueryResult.WhatIsYourInterestSection
                {
                    WhatIsYourInterest = application.WhatIsYourInterest
                },
                IsApplicationComplete = application.ApplicationAllSectionStatus.Equals("Completed", StringComparison.CurrentCultureIgnoreCase)
            };
        }
    }
}
