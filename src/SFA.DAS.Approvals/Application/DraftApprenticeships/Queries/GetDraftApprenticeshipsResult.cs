using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries
{
    public class GetDraftApprenticeshipsResult
    {
        public List<DraftApprenticeship> DraftApprenticeships { get; set; }

        public GetDraftApprenticeshipsResult()
        {
            DraftApprenticeships = new List<DraftApprenticeship>();
        }
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

        public static implicit operator DraftApprenticeship(InnerApi.Responses.DraftApprenticeship x)
        {
            return new DraftApprenticeship
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DateOfBirth = x.DateOfBirth,
                Cost = x.Cost,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Uln = x.Uln,
                CourseCode = x.CourseCode,
                CourseName = x.CourseName,
                DeliveryModel = (DeliveryModel)x.DeliveryModel,
                OriginalStartDate = x.OriginalStartDate,
            };
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryModel : byte
    {
        Regular = 0,
        PortableFlexiJob = 1,
    }
}
