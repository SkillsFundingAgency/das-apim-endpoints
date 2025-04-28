using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
public class GetProviderSummaryQueryResult
{
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
    public ProviderAddressModel ProviderAddress { get; set; }
    public ContactDetails Contact { get; set; }

    public ProviderQarModel Qar { get; set; }
    public ReviewsModel Reviews { get; set; }

    public List<CourseDetails> Courses { get; set; }

    public EndpointAssessmentsDetails EndpointAssessments { get; set; }

    public List<EmployerFeedbackStarsAnnualSummary> AnnualEmployerFeedbackDetails { get; set; }

    public List<ApprenticeFeedbackStarsAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
}

public class CourseDetails
{
    public string CourseName { get; set; }
    public int Level { get; set; }
    public int LarsCode { get; set; }
    public string IfateReferenceNumber { get; set; }

}

public class ContactDetails
{
    public string MarketingInfo { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
}

public class EndpointAssessmentsDetails
{
    public DateTime? EarliestAssessment { get; set; }
    public int EndpointAssessmentCount { get; set; }
}