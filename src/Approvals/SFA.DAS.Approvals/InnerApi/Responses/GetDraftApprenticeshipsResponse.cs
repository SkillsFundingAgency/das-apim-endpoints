using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetDraftApprenticeshipsResponse
    {
        public List<DraftApprenticeship> DraftApprenticeships { get; set; } = new List<DraftApprenticeship>();
    }

    public class DraftApprenticeship
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Uln { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public DateTime? OriginalStartDate { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryModel : byte
    {
        Regular = 0,
        PortableFlexiJob = 1,
        FlexiJobAgency = 2
    }
}
