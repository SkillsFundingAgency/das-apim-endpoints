using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostStopEmployerDemandRequest : IPostApiRequest
    {
        private readonly Guid _employerDemandId;

        public PostStopEmployerDemandRequest(Guid employerDemandId)
        {
            _employerDemandId = employerDemandId;
        }

        public string PostUrl => $"api/demand/{_employerDemandId}/stop";
        public object Data { get; set; }
    }
}