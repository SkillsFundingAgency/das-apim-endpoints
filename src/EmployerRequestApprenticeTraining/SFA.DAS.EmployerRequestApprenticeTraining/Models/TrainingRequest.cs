using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class TrainingRequest
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public DateTime RequestedAt { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public DateTime RemoveAt { get; set; }

        public List<Region> Regions { get; set; }
        public List<ProviderResponse> ProviderResponses { get; set; }

        public static explicit operator TrainingRequest(InnerApi.Responses.EmployerRequest source)
        {
            if (source == null) return null;

            return new TrainingRequest()
            {
                EmployerRequestId = source.Id,
                NumberOfApprentices = source.NumberOfApprentices,
                SameLocation = source.SameLocation,
                SingleLocation = source.SingleLocation,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                RequestedAt = source.RequestedAt,
                Status = source.RequestStatus,
                ExpiredAt = source.ExpiredAt,
                Regions = source.Regions.Select(s => (Region)s).ToList(),
                ProviderResponses = source.ProviderResponses.Select(s => (ProviderResponse)s).ToList(),
            };
        }
    }
}
