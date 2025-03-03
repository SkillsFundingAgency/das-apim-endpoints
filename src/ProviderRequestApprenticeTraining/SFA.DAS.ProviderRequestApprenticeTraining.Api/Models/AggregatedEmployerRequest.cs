using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class AggregatedEmployerRequest
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
        public bool IsNew { get; set; }

        public static implicit operator AggregatedEmployerRequest(GetAggregatedEmployerRequestsResponse source)
        {
            return new AggregatedEmployerRequest
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector,
                NumberOfApprentices = source.NumberOfApprentices,
                NumberOfEmployers = source.NumberOfEmployers,
                IsNew = source.IsNew,
            };
        }
    }
}
