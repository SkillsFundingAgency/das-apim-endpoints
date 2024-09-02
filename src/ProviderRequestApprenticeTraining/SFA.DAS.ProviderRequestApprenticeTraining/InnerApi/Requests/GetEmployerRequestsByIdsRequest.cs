using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class GetEmployerRequestsByIdsRequest : IGetApiRequest
    {
        public string GetUrl => GenerateUrl($"api/employerrequest", EmployerRequestIds);
        public List<Guid> EmployerRequestIds { get; set; }

        public GetEmployerRequestsByIdsRequest(List<Guid> employerRequestIds)
        {
            EmployerRequestIds = employerRequestIds;
        }

        private static string GenerateUrl(string baseUrl, List<Guid> employerRequestIds)
        {
            string queryString = string.Join("&", employerRequestIds.ConvertAll(id => "employerRequestIds=" + id));
            return $"{baseUrl}?{queryString}";
        }
    }
}
