using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
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
