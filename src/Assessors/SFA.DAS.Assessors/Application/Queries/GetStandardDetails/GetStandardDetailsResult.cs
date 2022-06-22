using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardDetails
{
    public class GetStandardDetailsResult
    {
        public StandardDetailResponse StandardDetails { get; }

        public GetStandardDetailsResult(StandardDetailResponse standardDetails)
        {
            StandardDetails = standardDetails;
        }
    }
}
