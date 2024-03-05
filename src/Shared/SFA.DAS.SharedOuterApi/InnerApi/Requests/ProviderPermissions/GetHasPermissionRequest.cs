using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, string operation) : IGetApiRequest
{
    public string GetUrl => $"has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operation={operation}";
}