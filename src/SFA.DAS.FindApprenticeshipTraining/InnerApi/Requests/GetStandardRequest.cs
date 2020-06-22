using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        private readonly int _id;

        public GetStandardRequest(int id)
        {
            _id = id;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/standards/{_id}";
    }
}