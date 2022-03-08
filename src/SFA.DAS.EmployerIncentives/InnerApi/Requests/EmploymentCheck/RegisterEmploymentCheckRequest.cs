using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck
{
    public class RegisterEmploymentCheckRequest : IPostApiRequest
    {
        public RegisterEmploymentCheckRequest(RegisterEmploymentCheckCommand data)
        {
            Data = data;
        }

        public string PostUrl => "api/EmploymentCheck/RegisterCheck";
        public object Data { get; set; }
    }
}
