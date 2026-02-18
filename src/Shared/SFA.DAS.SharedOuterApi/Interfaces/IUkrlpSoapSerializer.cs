using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Ukrlp;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IUkrlpSoapSerializer
    {
        List<Provider> DeserialiseMatchingProviderRecordsResponse(string soapXml);
        string BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime providerUpdatedSince, string stakeholderId, string queryId);
        string BuildGetAllUkrlpsFromUkprnsSoapRequest(List<long> ukprns, string stakeholderId, string queryId);
    }
}
