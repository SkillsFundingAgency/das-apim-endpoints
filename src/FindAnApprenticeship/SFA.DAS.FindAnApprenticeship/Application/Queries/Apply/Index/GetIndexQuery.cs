using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.FindAnApprenticeship.Domain.Constants;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public string ApplicantEmailAddress { get; set; }
        public string VacancyReference { get; set; }
    }

    public class GetIndexQueryResult
    {
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public EducationHistorySection EducationHistory { get; set; }
        public WorkHistorySection WorkHistory { get; set; }
        public ApplicationQuestionsSection ApplicationQuestions { get; set; }
        public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
        public DisabilityConfidenceSection DisabilityConfidence { get; set; }

        public class EducationHistorySection
        {
                public string Qualifications { get; set; }
                public string TrainingCourses { get; set; }
        }

        public class WorkHistorySection
        {
            public string Jobs { get; set; }
            public string VolunteeringAndWorkExperience { get; set; }
        }

        public class ApplicationQuestionsSection
        {
            public string SkillsAndStrengths { get; set; }
            public string WhatInterestsYou { get; set; }
            public string Travel { get; set; }
        }

        public class InterviewAdjustmentsSection
        {
            public string RequestAdjustments { get; set; }
        }
        public class DisabilityConfidenceSection
        {
            public string InterviewUnderDisabilityConfident { get; set; }
        }
    }

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
                    Qualifications = SectionStatus.NotYetStarted,
                    TrainingCourses = SectionStatus.NotYetStarted
                },
                WorkHistory = new GetIndexQueryResult.WorkHistorySection
                {
                    Jobs = SectionStatus.NotYetStarted,
                    VolunteeringAndWorkExperience = SectionStatus.NotYetStarted
                },
                ApplicationQuestions = new GetIndexQueryResult.ApplicationQuestionsSection
                {
                    SkillsAndStrengths = SectionStatus.NotYetStarted,
                    WhatInterestsYou = SectionStatus.NotYetStarted,
                    Travel = SectionStatus.NotYetStarted
                },
                InterviewAdjustments = new GetIndexQueryResult.InterviewAdjustmentsSection
                {
                    RequestAdjustments = SectionStatus.NotYetStarted
                },
                DisabilityConfidence = new GetIndexQueryResult.DisabilityConfidenceSection
                {
                    InterviewUnderDisabilityConfident = SectionStatus.NotYetStarted
                }
            };
        }
    }
}
