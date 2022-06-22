using SFA.DAS.SharedOuterApi.Interfaces;

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
