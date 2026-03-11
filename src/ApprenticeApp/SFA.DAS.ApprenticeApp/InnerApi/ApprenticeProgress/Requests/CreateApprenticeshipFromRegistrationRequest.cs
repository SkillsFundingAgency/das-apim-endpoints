using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class CreateApprenticeshipFromRegistrationRequest : IPostApiRequest
    {        
        public string PostUrl => $"/apprenticeships";
        public object Data { get; set; }

        public CreateApprenticeshipFromRegistrationRequest(CreateApprenticeshipFromRegistrationData data)
        {
            Data = data;
        }
    }
}
