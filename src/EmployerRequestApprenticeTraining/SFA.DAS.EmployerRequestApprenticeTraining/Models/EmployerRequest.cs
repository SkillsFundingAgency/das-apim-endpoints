using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Enums;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }

        public static explicit operator EmployerRequest(InnerApi.Responses.EmployerRequest source)
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
