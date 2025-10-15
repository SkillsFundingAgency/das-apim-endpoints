using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests
{
    public class GetApprenticeshipRegistrationByEmailRequest : IGetApiRequest
    {
        private readonly string _email;
        public GetApprenticeshipRegistrationByEmailRequest(string email)
        {
            _email = email;
        }
        public string GetUrl => $"registrations/{_email}/search";
    }
}
