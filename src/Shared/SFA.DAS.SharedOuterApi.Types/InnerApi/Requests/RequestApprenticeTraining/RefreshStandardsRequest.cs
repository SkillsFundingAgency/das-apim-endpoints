using System.Diagnostics.CodeAnalysis;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining
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
