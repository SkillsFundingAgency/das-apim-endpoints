using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Files;

[ExcludeFromCodeCoverage]
public class GetFileMetadataApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/files";

    public object Data { get; set; }
}