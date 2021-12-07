using System;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class CourseEpaoDetails
    {
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? DateStandardApprovedOnRegister { get; set; }
        public string[] StandardVersions { get; set; }
    }

}