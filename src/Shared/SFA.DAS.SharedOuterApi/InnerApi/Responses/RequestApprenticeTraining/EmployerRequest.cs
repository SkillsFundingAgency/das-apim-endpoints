using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public Status Status { get; set; }
        public Guid ModifiedBy { get; set; }

        public List<Region> Regions { get; set; }
    }
}
