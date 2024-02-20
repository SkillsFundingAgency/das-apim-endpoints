using System;
using static SFA.DAS.EmployerRequestApprenticeTraining.Models.Enums;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }

        public static explicit operator EmployerRequest(InnerApi.Responses.EmployerRequest source)
        {
            return new EmployerRequest()
            {
                Id = source.Id,
                RequestType = source.RequestType
            };
        }
    }
}
