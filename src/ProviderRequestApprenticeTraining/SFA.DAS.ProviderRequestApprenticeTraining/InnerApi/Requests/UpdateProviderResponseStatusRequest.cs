using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests
{
    public class UpdateProviderResponseStatusRequest : IPostApiRequest<UpdateProviderResponseStatusData>
    {
        public UpdateProviderResponseStatusData Data { get; set; }

        public string PostUrl => $"api/employerrequest/provider/responsestatus";

        public UpdateProviderResponseStatusRequest(UpdateProviderResponseStatusData data)
        {
            Data = data;
        }
    }
    public class UpdateProviderResponseStatusData 
    {
        public List<Guid> EmployerRequestIds { get; set; }
        public long Ukprn { get; set; }
        public int ProviderResponseStatus { get; set; }
    }
}
