using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CoursesApi;

public class GetCourseLookupResponse
    {
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public string Version { get; set; }
    public int VersionMajor { get; set; }
    public int VersionMinor { get; set; }
    public List<string> Options { get; set; }
    public StandardVersionDetail VersionDetail { get; set; }
    public string StandardPageUrl { get; set; }
    public string Route { get; set; }
    public string LearningType { get; set; }
}

