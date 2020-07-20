using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingLevelsListItem
    {
        public int Code { get ; set ; }

        public string Name { get ; set ; }

        public static implicit operator GetTrainingLevelsListItem(GetLevelsListItem levelsListItem)
        {
            return new GetTrainingLevelsListItem
            {
                Code = levelsListItem.Code,
                Name = levelsListItem.Name
            }; 
                
        }
    }
}