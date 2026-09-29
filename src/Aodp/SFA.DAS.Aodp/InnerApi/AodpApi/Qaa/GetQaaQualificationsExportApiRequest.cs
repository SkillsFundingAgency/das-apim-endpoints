using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qaa;

[ExcludeFromCodeCoverage]
public class GetQaaQualificationsExportApiRequest : IGetApiRequest
{
    public string CurrentUsername { get; set; } = string.Empty;
    public string GetUrl => $"api/qaa/download?username={Uri.EscapeDataString(CurrentUsername)}";
}
