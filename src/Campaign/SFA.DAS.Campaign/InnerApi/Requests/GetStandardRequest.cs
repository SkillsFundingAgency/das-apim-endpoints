using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Campaign.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        /// <summary>
        /// Can be StandardUId, LarsCode or IFateRef Number
        /// </summary>
        /// <param name="standardId"></param>
        public GetStandardRequest(string standardId)
        {
            StandardId = standardId;
        }

        public string StandardId { get; }
        public string GetUrl => $"api/courses/standards/{StandardId}";
    }
}