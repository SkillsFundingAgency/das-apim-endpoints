using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApprenticeRequest : IGetApiRequest
    {
        public Guid Id { get; }

        public GetApprenticeRequest(Guid id)
        {
            Id = id;
        }

        public string GetUrl => $"apprentices/{Id}";
    }
}
