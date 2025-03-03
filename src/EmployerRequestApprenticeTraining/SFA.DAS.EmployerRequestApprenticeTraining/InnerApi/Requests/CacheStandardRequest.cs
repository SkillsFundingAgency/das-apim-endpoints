using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.CacheStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class CacheStandardRequest : IPostApiRequest<CacheStandardRequestData>
    {
        public CacheStandardRequestData Data { get; set; }

        public CacheStandardRequest(CacheStandardRequestData data)
        {
            Data = data;
        }

        public string PostUrl => $"api/standards";

        public class CacheStandardRequestData
        {
            public string StandardReference { get; set; }
            public string StandardTitle { get; set; }
            public int StandardLevel { get; set; }
            public string StandardSector { get; set; }
        }

        public static implicit operator CacheStandardRequest(StandardDetailResponse source)
        {
            return new CacheStandardRequest(new CacheStandardRequestData 
            { 
                StandardLevel = source.Level,
                StandardReference = source.IfateReferenceNumber,
                StandardSector = source.Route,
                StandardTitle = source.Title,
            });
        }
    }
}
