using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Api.Models;

public class CloseVacancyRequest
{
    public required ClosureReason ClosureReason { get; set; }
}