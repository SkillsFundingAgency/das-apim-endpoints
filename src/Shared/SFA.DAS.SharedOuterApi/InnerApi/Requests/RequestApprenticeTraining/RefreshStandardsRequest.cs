using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class RefreshStandardsRequest : IPutApiRequest<RefreshStandardsData>
    {
        public string PutUrl => "api/standards/refresh";

        public RefreshStandardsData Data { get; set; }

        public RefreshStandardsRequest(RefreshStandardsData data)
        {
            Data = data;
        }
    }

    [ExcludeFromCodeCoverage]
    public class RefreshStandardsData
    {
        public List<StandardData> Standards { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class StandardData()
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
    }

}
