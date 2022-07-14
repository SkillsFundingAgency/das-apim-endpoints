using System;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Services
{
    public class ShortlistService : IShortlistService
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public ShortlistService (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<int> GetShortlistItemCount(Guid? userId)
        {
            if (!userId.HasValue || userId.Value == Guid.Empty)
            {
                return 0;
            }
            
            var result =
                await _courseDeliveryApiClient.Get<GetShortlistUserItemCountResponse>(
                    new GetShortlistUserItemCountRequest(userId.Value));

            return result?.Count ?? 0;
        }
    }
}