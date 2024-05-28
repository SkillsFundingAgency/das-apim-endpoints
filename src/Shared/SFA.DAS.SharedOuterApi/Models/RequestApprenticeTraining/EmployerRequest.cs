using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }

        public static explicit operator EmployerRequest(InnerApi.Responses.RequestApprenticeTraining.EmployerRequest source)
        {
            if (source == null) return null;

            return new EmployerRequest()
            {
                Id = source.Id,
                RequestType = source.RequestType,
                AccountId = source.AccountId
            };
        }
    }
}
