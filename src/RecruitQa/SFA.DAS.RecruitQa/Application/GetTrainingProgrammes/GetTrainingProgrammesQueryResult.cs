using SFA.DAS.RecruitQa.Domain;

namespace SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;

public record GetTrainingProgrammesQueryResult(IEnumerable<TrainingProgramme> _)
{
    public static readonly GetTrainingProgrammesQueryResult Empty = new([]);
};