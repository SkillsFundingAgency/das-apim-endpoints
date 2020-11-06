using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public int StandardId { get; set; }
        public string GetUrl => $"api/courses/standards/{StandardId}";
    }
}