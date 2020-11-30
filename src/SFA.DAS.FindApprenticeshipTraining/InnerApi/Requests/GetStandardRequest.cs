using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest // todo: remove once merged up to date
    {
        private readonly int _id;

        public GetStandardRequest(int id)
        {
            _id = id;
        }

        public string GetUrl => $"api/courses/standards/{_id}";
    }
}