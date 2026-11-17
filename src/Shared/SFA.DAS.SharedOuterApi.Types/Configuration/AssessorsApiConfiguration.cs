using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

[Obsolete("The misnamed Assessors configuration is being replaced with Assessor during the rewrite of the Assessor service")]
public class AssessorsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}