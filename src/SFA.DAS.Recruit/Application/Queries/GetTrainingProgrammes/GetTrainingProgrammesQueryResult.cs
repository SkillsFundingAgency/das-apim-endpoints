using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes
{
    public class GetTrainingProgrammesQueryResult
    {
        public IEnumerable<TrainingProgramme> TrainingProgrammes { get ; set ; }
    }
}