using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetTrainingProgrammesListResponse
    {
        public IEnumerable<GetTrainingProgrammeResponse> TrainingProgrammes { get; set; }
    }
}