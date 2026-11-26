using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

[ExcludeFromCodeCoverage]
public class RefreshStandardsRequest(RefreshStandardsData data) : IPutApiRequest<RefreshStandardsData>
{
    public string PutUrl => "api/standards/refresh";

    public RefreshStandardsData Data { get; set; } = data;
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