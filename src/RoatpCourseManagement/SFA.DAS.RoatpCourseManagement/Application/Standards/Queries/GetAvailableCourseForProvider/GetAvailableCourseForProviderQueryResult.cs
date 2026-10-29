using System;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCourseForProvider;

public class GetAvailableCourseForProviderQueryResult
{
    public string LarsCode { get; set; }
    public int Level { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public string ApprovalBody { get; set; }
    public DateTime? LastDateStarts { get; set; }
}
