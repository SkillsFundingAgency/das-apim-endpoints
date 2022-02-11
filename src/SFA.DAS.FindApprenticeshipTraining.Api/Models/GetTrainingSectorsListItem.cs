using System;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingSectorsListItem
    {
        public string Route { get ; set ; }

        public int Id { get ; set ; }

        public static implicit operator GetTrainingSectorsListItem(GetRoutesListItem routesListItem)
        {
            return new GetTrainingSectorsListItem
            {
                Id = routesListItem.Id,
                Route = routesListItem.Name
            };
        }
    }
}