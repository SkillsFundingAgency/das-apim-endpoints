using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobByNameApiRequest : IGetApiRequest
    {
        private readonly string Name;

        public GetJobByNameApiRequest(string name)
        {
            Name = name;
        }

        public string GetUrl => $"api/job/?name={Name}";
    }
}
