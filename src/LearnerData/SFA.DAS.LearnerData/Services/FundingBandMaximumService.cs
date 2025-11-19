using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Services
{
    public class FundingBandMaximumService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IFundingBandMaximumService
    {
        public async Task<int> GetFundingBandMaximum(int standardCode, DateTime effectiveDate)
        {
            var response = await coursesApiClient.Get<StandardDetailResponse>(
                new GetStandardDetailsByIdRequest(standardCode.ToString()));

            return response.MaxFundingOn(effectiveDate);
        }
    }
}
