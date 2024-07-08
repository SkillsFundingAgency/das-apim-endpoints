using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class CreateEmployerRequestRequest : IPostApiRequest<CreateEmployerRequestData>
    {
        public string PostUrl => "api/employerrequest";

        public CreateEmployerRequestData Data { get; set; }

        public CreateEmployerRequestRequest(CreateEmployerRequestData data)
        {
            Data = data;
        }
    }

    [ExcludeFromCodeCoverage]
    public class CreateEmployerRequestData
    {
        public string OriginalLocation { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public double SingleLocationLatitude { get; set; }
        public double SingleLocationLongitude { get; set; }
        public string[] MultipleLocations { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
