using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Responses
{
    public class GetAllCoursesResponse
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public int LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string StandardInfoUrl { get; set; }
        public string ContactUsPhoneNumber { get; set; }
        public string ContactUsEmail { get; set; }
        public string ContactUsPageUrl { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public bool IsImported { get; set; } = false;
        public bool? IsConfirmed { get; set; } //required if imported
        public bool? HasNationalDeliveryOption { get; set; }
        public bool? HasHundredPercentEmployerDeliveryOption { get; set; }
        public List<DeliveryModel> DeliveryModels { get; set; } = new List<DeliveryModel>();
    }
    public enum DeliveryModel
    {
        Regular,
        PortableFlexiJob
    }
}