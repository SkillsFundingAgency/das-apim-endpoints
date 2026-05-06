namespace SFA.DAS.RecruitQa.Api.Models.Responses;

public class GetTrainingProgrammesListResponse
{
    public IEnumerable<GetTrainingProgrammeResponse> TrainingProgrammes { get; set; }
}