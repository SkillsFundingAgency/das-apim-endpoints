using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class GetRolloverStartSummaryApiRequest() : IGetApiRequest
{
    public string GetUrl => "api/rollover/startsummary";

}
