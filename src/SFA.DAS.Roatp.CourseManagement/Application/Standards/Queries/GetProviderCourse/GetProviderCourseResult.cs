using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse
{
    public class GetProviderCourseResult
    {
        public string CourseName { get; set; }
        public int Level { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string Version { get; set; }
        public string RegulatorName { get; set; }
        public int LarsCode { get; set; }
        public string Sector { get; set; }
        public string StandardInfoUrl { get; set; }
        public string ContactUsPhoneNumber { get; set; }
        public string ContactUsEmail { get; set; }
        public string ContactUsPageUrl { get; set; }
        public List<ProviderCourseLocationModel> ProviderCourseLocations { get; set; } = new List<ProviderCourseLocationModel>();
        public bool? IsApprovedByRegulator { get; set; }
    }
}
