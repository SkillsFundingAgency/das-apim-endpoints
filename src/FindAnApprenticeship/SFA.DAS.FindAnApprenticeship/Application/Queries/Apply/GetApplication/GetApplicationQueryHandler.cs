using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId));
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

            await Task.WhenAll(candidateTask, addressTask);

            var candidate = candidateTask.Result;
            var address = addressTask.Result;

            return new GetApplicationQueryResult
            {
                IsDisabilityConfident = vacancy.IsDisabilityConfident,
                EducationHistory = new GetApplicationQueryResult.EducationHistorySection
                {
                    QualificationsStatus = application.QualificationsStatus,
                    TrainingCoursesStatus = application.TrainingCoursesStatus,
                },
                WorkHistory = new GetApplicationQueryResult.WorkHistorySection
                {
                    JobsStatus = application.JobsStatus,
                    VolunteeringAndWorkExperienceStatus = application.WorkExperienceStatus,
                },
                ApplicationQuestions = new GetApplicationQueryResult.ApplicationQuestionsSection
                {
                    SkillsAndStrengthsStatus = application.SkillsAndStrengthStatus,
                    WhatInterestsYouStatus = application.InterestsStatus,
                    AdditionalQuestion1Status = application.AdditionalQuestion1Status,
                    AdditionalQuestion1Id = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(0) != null ? additionalQuestions[0].Id : null,
                    AdditionalQuestion2Status = application.AdditionalQuestion2Status,
                    AdditionalQuestion2Id = additionalQuestions is { Count: > 0 } && additionalQuestions.ElementAtOrDefault(1) != null ? additionalQuestions[1].Id : null
                },
                InterviewAdjustments = new GetApplicationQueryResult.InterviewAdjustmentsSection
                {
                    RequestAdjustmentsStatus = application.InterviewAdjustmentsStatus
                },
                DisabilityConfidence = new GetApplicationQueryResult.DisabilityConfidenceSection
                {
                    InterviewUnderDisabilityConfidentStatus = application.DisabilityConfidenceStatus,
                },
                CandidateDetails = new GetApplicationQueryResult.Candidate
                {
                    Id = candidate.Id,
                    GovUkIdentifier = candidate.GovUkIdentifier,
                    Email = candidate.Email,
                    FirstName = candidate.FirstName,
                    LastName = candidate.LastName,
                    MiddleName = candidate.MiddleName,
                    PhoneNumber = candidate.PhoneNumber,
                    DateOfBirth = candidate.DateOfBirth,
                    Address = address
                }
            };
        }
    }
}
