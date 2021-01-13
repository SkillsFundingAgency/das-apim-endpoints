using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Application.Queries.GetStandards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<TrainingProgramme> TrainingProgrammes { get ; set ; }
    }
}