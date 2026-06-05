using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class ValidateFundingExtensionCandidatesApiRequest : IPostApiRequest
{
    public string PostUrl => "api/rollover/rolloverextensionvalidation";

    public object Data { get; set; }
}
