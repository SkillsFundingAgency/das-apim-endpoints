using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostPausePaymentsRequest : IPostApiRequest<PausePaymentsRequest>
    {
        public PostPausePaymentsRequest(PausePaymentsRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"pause-payments";
        public PausePaymentsRequest Data { get; set; }
    }

    public enum PausePaymentsAction
    {
        Pause = 1,
        Resume = 2
    }

    public class PausePaymentsRequest
    {
        public PausePaymentsAction Action { get; set; }
        public Application[] Applications { get; set; }
        public ServiceRequest ServiceRequest { get; set; }       
    }
}