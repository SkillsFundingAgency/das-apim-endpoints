using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public RequestStatus Status { get; set; }
        public Guid ModifiedBy { get; set; }

        public List<Region> Regions { get; set; }

        public static explicit operator EmployerRequest(InnerApi.Responses.RequestApprenticeTraining.EmployerRequest source)
        {
            if (source == null) return null;

            return new EmployerRequest()
            {
                Id = source.Id,
                RequestType = source.RequestType,
                AccountId = source.AccountId,
                StandardReference = source.StandardReference,
                NumberOfApprentices = source.NumberOfApprentices,
                SameLocation = source.SameLocation,
                SingleLocation = source.SingleLocation,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                RequestedBy = source.RequestedBy,
                Status = source.Status,
                ModifiedBy = source.ModifiedBy,
                Regions = source.Regions.Select(s => (Region)s).ToList()
            };
        }
    }
}
