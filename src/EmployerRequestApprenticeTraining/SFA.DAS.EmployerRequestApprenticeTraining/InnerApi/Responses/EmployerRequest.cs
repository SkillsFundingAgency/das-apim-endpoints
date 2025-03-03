using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses
{
    [ExcludeFromCodeCoverage]
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
        public RequestStatus RequestStatus { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public List<Region> Regions { get; set; }
        public List<ProviderResponse> ProviderResponses { get; set; }
    }
}
