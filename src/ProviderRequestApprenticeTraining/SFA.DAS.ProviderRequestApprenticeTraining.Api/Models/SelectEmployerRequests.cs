using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
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

        public static implicit operator SelectEmployerRequests(GetSelectEmployerRequestsResult source)
        {
            return new SelectEmployerRequests
            {
                StandardReference = source.SelectEmployerRequests.FirstOrDefault().StandardReference,
                StandardTitle = source.SelectEmployerRequests.FirstOrDefault().StandardTitle,
                StandardLevel = source.SelectEmployerRequests.FirstOrDefault().StandardLevel,
                EmployerRequests = source.SelectEmployerRequests.Select(request => (SelectEmployerRequest)request).ToList()
            };
        }
    }
}
