using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
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
        public DateTime RequestedAt { get; set; }
        public Guid RequestedBy { get; set; }
        public RequestStatus Status { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public List<Region> Regions { get; set; }

        public List<ProviderResponse> ProviderResponses { get; set; }

        public static explicit operator EmployerRequest(InnerApi.Responses.EmployerRequest source)
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
                RequestedAt = source.RequestedAt,
                RequestedBy = source.RequestedBy,
                Status = source.Status,
                ModifiedBy = source.ModifiedBy,
                ExpiredAt = source.ExpiredAt,
                Regions = source.Regions.Select(s => (Region)s).ToList(),
                ProviderResponses = source.ProviderResponses.Select(s => (ProviderResponse)s).ToList()
            };
        }
    }
}
