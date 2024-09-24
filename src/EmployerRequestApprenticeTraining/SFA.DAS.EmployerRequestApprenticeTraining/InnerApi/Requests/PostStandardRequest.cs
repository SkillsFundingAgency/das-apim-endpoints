using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class PostStandardRequest : IPostApiRequest<PostStandardRequestData>
    {
        public PostStandardRequestData Data { get; set; }

        public PostStandardRequest(PostStandardRequestData data)
        {
            Data = data;
        }

        public string PostUrl => $"api/standards";

        public class PostStandardRequestData
        {
            public string StandardReference { get; set; }
            public string StandardTitle { get; set; }
            public int StandardLevel { get; set; }
            public string StandardSector { get; set; }
        }

        public static implicit operator PostStandardRequest(StandardDetailResponse source)
        {
            return new PostStandardRequest(new PostStandardRequestData 
            { 
                StandardLevel = source.Level,
                StandardReference = source.IfateReferenceNumber,
                StandardSector = source.Route,
                StandardTitle = source.Title,
            });
        }
    }
}
