using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunByIdApiRequest : IGetApiRequest
    {
        private readonly Guid Id;

        public GetJobRunByIdApiRequest(Guid id)
        {
            Id = id;
        }

        public string GetUrl => $"api/job/jobruns/{Id}";
    }
}
