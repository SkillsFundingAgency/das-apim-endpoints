using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        private readonly int _id;

        public GetStandardRequest(int id)
        {
            _id = id;
        }

        public string GetUrl => $"api/courses/standards/{_id}";
    }
}