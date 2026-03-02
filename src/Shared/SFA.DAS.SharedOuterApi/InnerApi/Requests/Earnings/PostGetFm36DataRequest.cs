using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings
{
    /// <summary>
    /// Gets fm36 data for a list of learning keys. If learning keys are not sent then all fm36 data for the provider will be returned.
    /// </summary>
    /// <remarks>Although this is a Post, no data is added or modified in the earnings inner domain</remarks>
    public class PostGetFm36DataRequest : IPostApiRequest
    {
        public long Ukprn { get; }
        public int CollectionYear { get; set; }
        public byte CollectionPeriod { get; set; }

        public string PostUrl => $"{Ukprn}/fm36/{CollectionYear}/{CollectionPeriod}";

        public object Data { get; set; }

        public PostGetFm36DataRequest(long ukprn, int collectionYear, byte collectionPeriod, List<Guid> learningKeys)
        {
            Ukprn = ukprn;
            CollectionYear = collectionYear;
            CollectionPeriod = collectionPeriod;
            Data = learningKeys;
        }
    }
}