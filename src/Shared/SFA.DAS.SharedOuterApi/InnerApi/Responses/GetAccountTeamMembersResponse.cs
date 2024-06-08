using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAccountTeamMembersResponse
    {
        public string UserRef { get; set; }
        public string Role { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    }
}