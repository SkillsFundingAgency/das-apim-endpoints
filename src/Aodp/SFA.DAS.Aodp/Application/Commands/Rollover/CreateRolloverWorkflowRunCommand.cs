using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class CreateRolloverWorkflowRunCommand : IRequest<BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>>
    {
        public string AcademicYear { get; set; } = null!;
        public SelectionMethod SelectionMethod { get; set; }
        public List<Guid> RolloverCandidateIds { get; set; } = new();
        public DateTime? FundingEndDateEligibilityThreshold { get; set; }
        public DateTime? OperationalEndDateEligibilityThreshold { get; set; }
        public DateTime? MaximumApprovalFundingEndDate { get; set; }
        public string? CreatedByUserName { get; set; }
    }

    public enum SelectionMethod
    {
        None = 0,
        QueryBuilder = 1,
        FileUpload = 2,
        Unknown = 99
    }
}