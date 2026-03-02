using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetAllProviderCoursesResponse
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public string LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string StandardInfoUrl { get; set; }
        public string ContactUsPhoneNumber { get; set; }
        public string ContactUsEmail { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public bool? IsConfirmed { get; set; } //required if imported
        public bool? HasNationalDeliveryOption { get; set; }
        public bool? HasHundredPercentEmployerDeliveryOption { get; set; }
        public List<DeliveryModel> DeliveryModels { get; set; } = new List<DeliveryModel>();
        public string ApprovalBody { get; set; }
        public bool IsRegulatedForProvider { get; set; }
        public bool HasLocations { get; set; }
        public bool HasOnlineDeliveryOption { get; set; }
    }
    public enum DeliveryModel
    {
        Regular,
        PortableFlexiJob
    }
}