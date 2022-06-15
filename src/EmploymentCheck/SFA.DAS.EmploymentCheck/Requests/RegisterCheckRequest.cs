using SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Requests
{
    public class RegisterCheckRequest : IPostApiRequest
    {
        public RegisterCheckRequest(RegisterCheckCommand data)
        {
            Data = data;
        }

        public string PostUrl => "api/EmploymentCheck/RegisterCheck";
        public object Data { get; set; }
    }
}
