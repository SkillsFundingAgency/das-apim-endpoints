using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetShowEmployerDemandRequest : IGetApiRequest
    {
        public string GetUrl => "api/demand/show";
    }
}