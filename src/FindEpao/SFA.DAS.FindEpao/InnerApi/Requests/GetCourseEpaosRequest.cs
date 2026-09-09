using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetCourseEpaosRequest : IGetAllApiRequest
    {
        public int CourseId { get; set; }
        public string GetAllUrl => $"api/v1/standards/{CourseId}/organisations";
    }
}