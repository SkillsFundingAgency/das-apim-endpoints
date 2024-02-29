using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
    }
}
