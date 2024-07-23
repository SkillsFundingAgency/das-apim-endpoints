using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses
{
    public class GetAggregatedEmployerRequestsResponse 
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
        public bool IsNew { get; set; }
    }
}
    