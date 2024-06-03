using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetLegacyApplications;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetLegacyApplicationsApiResponse
    {
        public List<Application> Applications { get; set; }

        public static implicit operator GetLegacyApplicationsApiResponse(GetLegacyApplicationsQueryResult source)
        {
            return new GetLegacyApplicationsApiResponse
            {
                Applications = source.Applications.Select(x => (Application)x).ToList()
            };
        }

        public class Application
        {
            public ApplicationStatus Status { get; set; }
            public DateTime? DateApplied { get; set; }
            public DateTime? SuccessfulDateTime { get; set; }
            public DateTime? UnsuccessfulDateTime { get; set; }
            public string UnsuccessfulReason { get; set; }
            public string AdditionalQuestion1Answer { get; set; }
            public string AdditionalQuestion2Answer { get; set; }

            public Vacancy Vacancy { get; set; }
            public CandidateInformation CandidateInformation { get; set; }

            public static implicit operator Application(GetLegacyApplicationsByEmailApiResponse.Application application)
            {
                return new Application
                {
                    AdditionalQuestion1Answer = application.AdditionalQuestion1Answer,
                    AdditionalQuestion2Answer = application.AdditionalQuestion2Answer,
                    CandidateInformation = application.CandidateInformation,
                    DateApplied = application.DateApplied,
                    Status = (ApplicationStatus)application.Status,
                    SuccessfulDateTime = application.SuccessfulDateTime,
                    UnsuccessfulDateTime = application.UnsuccessfulDateTime,
                    UnsuccessfulReason = application.UnsuccessfulReason,
                    Vacancy = application.Vacancy
                };
            }
        }

        public class Vacancy
        {
            public long Id { get; set; }
            public string VacancyReference { get; set; }

            public static implicit operator Vacancy(GetLegacyApplicationsByEmailApiResponse.Vacancy source)
            {
                return new Vacancy
                {
                    Id = source.Id,
                    VacancyReference = source.VacancyReference
                };
            }
        }

        public class CandidateInformation
        {
            public List<Qualification> Qualifications { get; set; } = null!;
            public List<WorkExperience> WorkExperiences { get; set; } = null!;
            public List<TrainingCourse> TrainingCourses { get; set; } = null!;
            public AboutYou AboutYou { get; set; }

            public static implicit operator CandidateInformation(GetLegacyApplicationsByEmailApiResponse.CandidateInformation source)
            {
                return new CandidateInformation
                {
                    AboutYou = source.AboutYou,
                    Qualifications = source.Qualifications.Select(x => (Qualification)x).ToList(),
                    WorkExperiences = source.WorkExperiences.Select(x => (WorkExperience)x).ToList(),
                    TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                };
            }
        }

        public class Qualification
        {
            public string QualificationType { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public bool IsPredicted { get; set; }
            public int Year { get; set; }

            public static implicit operator Qualification(GetLegacyApplicationsByEmailApiResponse.Qualification source)
            {
                return new Qualification
                {
                   QualificationType = source.QualificationType,
                   Subject = source.Subject,
                   Grade = source.Grade,
                   IsPredicted = source.IsPredicted,
                   Year = source.Year
                };
            }
        }

        public class WorkExperience
        {
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public string Description { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

            public static implicit operator WorkExperience(GetLegacyApplicationsByEmailApiResponse.WorkExperience source)
            {
                return new WorkExperience
                {
                    Employer = source.Employer,
                    Description = source.Description,
                    JobTitle = source.JobTitle,
                    FromDate = source.FromDate,
                    ToDate = source.ToDate
                };
            }
        }

        public class TrainingCourse
        {
            public string Provider { get; set; }
            public string Title { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

            public static implicit operator TrainingCourse(GetLegacyApplicationsByEmailApiResponse.TrainingCourse source)
            {
                return new TrainingCourse
                {
                    Provider = source.Provider,
                    Title = source.Title,
                    FromDate = source.FromDate,
                    ToDate = source.ToDate
                };
            }
        }

        public class AboutYou
        {
            public string Strengths { get; set; }

            public string Improvements { get; set; }

            public string HobbiesAndInterests { get; set; }

            public string Support { get; set; }

            public static implicit operator AboutYou(GetLegacyApplicationsByEmailApiResponse.AboutYou source)
            {
                return new AboutYou
                {
                    Strengths = source.Strengths,
                    Improvements = source.Improvements,
                    HobbiesAndInterests = source.HobbiesAndInterests,
                    Support = source.Support
                };
            }
        }
    }
}
