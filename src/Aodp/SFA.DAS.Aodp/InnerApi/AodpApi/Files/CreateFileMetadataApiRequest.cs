using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Files;

[ExcludeFromCodeCoverage]
public class CreateFileMetadataApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/files/create";

    public object Data { get; set; }
}