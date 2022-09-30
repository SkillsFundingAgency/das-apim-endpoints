using System;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public interface IUkrlpSoapSerializer
    {
        string BuildUkrlpSoapRequest(long ukprn, string stakeholderId, string queryId);
        List<MatchingProviderRecords> DeserialiseMatchingProviderRecordsResponse(string soapXml);
        string BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime providerUpdatedSince, string stakeholderId, string queryId);
        string BuildGetAllUkrlpsFromUkprnsSoapRequest(List<long> ukprns, string stakeholderId, string queryId);
    }
}
