using System;
using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQueryResult
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public ShortProviderAddressModel ProviderAddress { get; set; }
    public ContactModel Contact { get; set; }
    public string CourseName { get; set; }
    public CourseType CourseType { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public int Level { get; set; }
    public string LarsCode { get; set; }
    public string IFateReferenceNumber { get; set; }
    public QarModel Qar { get; set; }
    public ReviewModel Reviews { get; set; }
    public IEnumerable<LocationModel> Locations { get; set; }
    public IEnumerable<ProviderCourseModel> Courses { get; set; } = [];
    public IEnumerable<EmployerFeedbackStarsAnnualSummary> AnnualEmployerFeedbackDetails { get; set; } = [];
    public IEnumerable<ApprenticeFeedbackStarsAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; } = [];
    public EndpointAssessmentModel EndpointAssessments { get; set; }
    public int TotalProvidersCount { get; set; }
    public Guid? ShortlistId { get; set; }

    public static implicit operator GetCourseProviderQueryResult(GetCourseProviderDetailsResponse source)
    {
        return new GetCourseProviderQueryResult
        {
            Ukprn = source.Ukprn,
            ProviderName = source.ProviderName,
            ProviderAddress = source.Address,
            Contact = source.Contact,
            CourseName = source.CourseName,
            CourseType = source.CourseType,
            ApprenticeshipType = source.ApprenticeshipType,
            Level = source.Level,
            LarsCode = source.LarsCode,
            IFateReferenceNumber = source.IFateReferenceNumber,
            Qar = source.QAR,
            Reviews = source.Reviews,
            ShortlistId = source.ShortlistId,
            Locations = source.Locations
        };
    }
}
