using System;
using static SFA.DAS.EmployerRequestApprenticeTraining.Models.Enums;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public RequestType RequestType { get; set; }
    }
}
