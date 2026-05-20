using System;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetEmployerDemandRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetEmployerDemandRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"api/demand/{_id}";
    }
}