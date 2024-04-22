using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication
{
    public class GetApplicationQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApplicationQuery, GetApplicationQueryResult>
    {
        public async Task<GetApplicationQueryResult> Handle(
        GetApplicationQuery request,
            CancellationToken cancellationToken)
        {
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));
            if (application == null) return null;

            var vacancy = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(application.VacancyReference));
            if (vacancy == null) return null;

            var additionalQuestions = application.AdditionalQuestions.ToList();

            var candidateTask =
                candidateApiClient.Get<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(request.CandidateId.ToString()));

            var addressTask =
                candidateApiClient.Get<GetAddressApiResponse>(
                    new GetCandidateAddressApiRequest(request.CandidateId));

            await Task.WhenAll(
                candidateTask,
                addressTask
                );

            var candidate = candidateTask.Result;
            var address = addressTask.Result;
            var trainingCourses = application.TrainingCourses;
            var jobs = application.WorkHistory.Where(c=>c.WorkHistoryType == WorkHistoryType.Job);
            var volunteeringExperiences = application.WorkHistory.Where(c=>c.WorkHistoryType == WorkHistoryType.WorkExperience);
            var otherDetails = application.AboutYou;

            GetApplicationQueryResult.ApplicationQuestionsSection.Question additionalQuestion1 = null;
            GetApplicationQueryResult.ApplicationQuestionsSection.Question additionalQuestion2 = null;

            var additionalQuestion1Id =
                additionalQuestions is {Count: > 0} && additionalQuestions.ElementAtOrDefault(0) != null
                    ? additionalQuestions[0].Id
                    : (Guid?) null;

            if (additionalQuestion1Id != null)
            {
                var additionalQuestion = await candidateApiClient.Get<GetAdditionalQuestionApiResponse>(new GetAdditionalQuestionApiRequest(request.ApplicationId, request.CandidateId, (Guid)additionalQuestion1Id));
                additionalQuestion1 = new GetApplicationQueryResult.ApplicationQuestionsSection.Question
                {
                    Id = additionalQuestion.Id,
                    Answer = additionalQuestion.Answer,
                    QuestionLabel = additionalQuestion.QuestionText,
                    Status = application.AdditionalQuestion1Status
                };
            }

            var additionalQuestion2Id =
                additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null
                    ? additionalQuestions[1].Id
                    : (Guid?)null;

            if (additionalQuestion2Id != null)
            {
                var additionalQuestion = await candidateApiClient.Get<GetAdditionalQuestionApiResponse>(new GetAdditionalQuestionApiRequest(request.ApplicationId, request.CandidateId, (Guid)additionalQuestion2Id));
                additionalQuestion2 = new GetApplicationQueryResult.ApplicationQuestionsSection.Question
                {
                    Id = additionalQuestion.Id,
                    Answer = additionalQuestion.Answer,
                    QuestionLabel = additionalQuestion.QuestionText,
                    Status = application.AdditionalQuestion1Status
                };
            }

            return new GetApplicationQueryResult
            {
                IsDisabilityConfident = vacancy.IsDisabilityConfident,
                EducationHistory = new GetApplicationQueryResult.EducationHistorySection
                {
                    QualificationsStatus = application.QualificationsStatus,
                    TrainingCoursesStatus = application.TrainingCoursesStatus,
                    TrainingCourses = trainingCourses.Select(x => (GetApplicationQueryResult.EducationHistorySection.TrainingCourse)x).ToList()
                },
                WorkHistory = new GetApplicationQueryResult.WorkHistorySection
                {
                    JobsStatus = application.JobsStatus,
                    VolunteeringAndWorkExperienceStatus = application.WorkExperienceStatus,
                    Jobs = jobs.Select(x => (GetApplicationQueryResult.WorkHistorySection.Job)x).ToList(),
                    VolunteeringAndWorkExperiences = volunteeringExperiences.Select(x => (GetApplicationQueryResult.WorkHistorySection.VolunteeringAndWorkExperience)x).ToList()
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
                    InterviewAdjustmentsDescription = otherDetails?.Support,
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
                AboutYou = new GetApplicationQueryResult.AboutYouSection
                {
                    HobbiesAndInterests = otherDetails?.HobbiesAndInterests,
                    Improvements = otherDetails?.Improvements,
                    SkillsAndStrengths = otherDetails?.SkillsAndStrengths,
                    Support = otherDetails?.Support
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
