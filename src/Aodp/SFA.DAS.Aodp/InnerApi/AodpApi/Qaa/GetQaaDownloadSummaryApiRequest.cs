using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qaa;

[ExcludeFromCodeCoverage]
public class GetQaaDownloadSummaryApiRequest : IGetApiRequest
{
    public string GetUrl => "api/qaa/download-summary";
}
