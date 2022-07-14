using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck
{
    public class UpdateEmploymentCheckRequest : IPutApiRequest<UpdateRequest>
    {
        public string PutUrl => $"employmentchecks/{Data.CorrelationId}";
        public UpdateRequest Data { get; set; }
    }

    public class UpdateRequest
    {
        public Guid CorrelationId { get; set; }
        public string Result { get; set; }
        public DateTime DateChecked { get; set; }
    }
}
