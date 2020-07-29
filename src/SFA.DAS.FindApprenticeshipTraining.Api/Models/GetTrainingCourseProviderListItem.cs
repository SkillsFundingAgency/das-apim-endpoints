using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderListItem
    {
        public string Name { get ; set ; }

        public int ProviderId { get ; set ; }

        public static implicit operator GetTrainingCourseProviderListItem(GetProvidersListItem source)
        {
            return new GetTrainingCourseProviderListItem
            {
                Name = source.Name,
                ProviderId = source.Ukprn
            };
        }
    }
}