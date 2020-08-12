using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutJobRequest : IPutApiRequest
    {
        public PutJobRequest(JobRequest request)
        {
            Data = request;
        }
        public string BaseUrl { get; set; }
        public string PutUrl => $"{BaseUrl}jobs";
        public object Data { get; set; }
    }
}
