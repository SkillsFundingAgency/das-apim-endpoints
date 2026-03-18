using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutJobRequest : IPutApiRequest
    {
        public PutJobRequest(JobRequest request)
        {
            Data = request;
        }
        public string PutUrl => "jobs";
        public object Data { get; set; }
    }
}
