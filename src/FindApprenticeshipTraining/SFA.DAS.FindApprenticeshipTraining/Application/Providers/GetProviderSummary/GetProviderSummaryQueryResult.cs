using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;
public class GetProviderSummaryQueryResult
{
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
    public ProviderAddressDetails ProviderAddress { get; set; }
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
    public string LarsCode { get; set; }
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

public class ProviderAddressDetails
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Town { get; set; }
    public string Postcode { get; set; }

    public static implicit operator ProviderAddressDetails(ProviderAddressModel source)
    {
        if (source == null)
            return null;

        return new ProviderAddressDetails
        {
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            AddressLine3 = source.AddressLine3,
            AddressLine4 = source.AddressLine4,
            Postcode = source.Postcode,
            Town = source.Town
        };
    }
}