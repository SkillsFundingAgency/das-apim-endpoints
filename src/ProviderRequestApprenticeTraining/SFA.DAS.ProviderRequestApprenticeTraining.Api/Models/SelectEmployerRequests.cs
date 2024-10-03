using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class SelectEmployerRequests
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<SelectEmployerRequest> EmployerRequests { get; set; }
        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterRequestedMonths { get; set; }

        public static implicit operator SelectEmployerRequests(GetSelectEmployerRequestsResult source)
        {
            if (source.SelectEmployerRequests.Any())
            {
                return new SelectEmployerRequests
                {
                    StandardReference = source.SelectEmployerRequests.First().StandardReference,
                    StandardTitle = source.SelectEmployerRequests.First().StandardTitle,
                    StandardLevel = source.SelectEmployerRequests.First().StandardLevel,
                    EmployerRequests = source.SelectEmployerRequests.Select(request => (SelectEmployerRequest)request).ToList()
                };
            }
            else
            {
                return new SelectEmployerRequests 
                {
                    EmployerRequests = new List<SelectEmployerRequest>()
                };
            }
        }
    }
}
