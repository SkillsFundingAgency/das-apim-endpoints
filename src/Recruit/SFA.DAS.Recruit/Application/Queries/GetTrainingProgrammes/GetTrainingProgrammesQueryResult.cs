using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;

public record GetTrainingProgrammesQueryResult(IEnumerable<TrainingProgramme> TrainingProgrammes)
{
    public static readonly GetTrainingProgrammesQueryResult Empty = new([]);
};