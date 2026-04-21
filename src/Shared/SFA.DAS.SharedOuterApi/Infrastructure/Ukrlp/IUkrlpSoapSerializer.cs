
using SFA.DAS.SharedOuterApi.Types.Infrastructure.Ukrlp;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp
{
    public interface IUkrlpSoapSerializer
    {
        List<Provider> DeserialiseMatchingProviderRecordsResponse(string soapXml);
        string BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime providerUpdatedSince, string stakeholderId, string queryId);
        string BuildGetAllUkrlpsFromUkprnsSoapRequest(List<long> ukprns, string stakeholderId, string queryId);
    }
}
