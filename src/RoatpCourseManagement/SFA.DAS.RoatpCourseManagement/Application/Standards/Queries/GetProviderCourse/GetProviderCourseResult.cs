using System.Collections.Generic;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;

public class GetProviderCourseResult
{
    public string CourseName { get; set; }
    public int Level { get; set; }
    public string IfateReferenceNumber { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public string RegulatorName { get; set; }
    public string LarsCode { get; set; }
    public string Sector { get; set; }
    public string StandardInfoUrl { get; set; }
    public string ContactUsPhoneNumber { get; set; }
    public string ContactUsEmail { get; set; }
    public List<ProviderCourseLocationModel> ProviderCourseLocations { get; set; } = new List<ProviderCourseLocationModel>();
    public bool? IsApprovedByRegulator { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public bool HasLocations { get; set; }
    public bool HasOnlineDeliveryOption { get; set; }

}