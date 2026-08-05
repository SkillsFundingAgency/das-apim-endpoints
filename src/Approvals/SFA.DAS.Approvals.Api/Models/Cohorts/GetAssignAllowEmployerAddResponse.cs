using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAssignAllowEmployerAdd;

namespace SFA.DAS.Approvals.Api.Models.Cohorts;

public class GetAssignAllowEmployerAddResponse
{
    public byte? LearningType { get; set; }
    public bool AllowEmployerAdd { get; set; }

    public static implicit operator GetAssignAllowEmployerAddResponse(GetAssignAllowEmployerAddQueryResult source)
    {
        return new GetAssignAllowEmployerAddResponse
        {
            LearningType = source.LearningType,
            AllowEmployerAdd = source.AllowEmployerAdd
        };
    }
}
