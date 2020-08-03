using System;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingSectorsListItem
    {
        public string Route { get ; set ; }

        public Guid Id { get ; set ; }

        public static implicit operator GetTrainingSectorsListItem(GetSectorsListItem sectorsListItem)
        {
            return new GetTrainingSectorsListItem
            {
                Id = sectorsListItem.Id,
                Route = sectorsListItem.Route
            };
        }
    }
}