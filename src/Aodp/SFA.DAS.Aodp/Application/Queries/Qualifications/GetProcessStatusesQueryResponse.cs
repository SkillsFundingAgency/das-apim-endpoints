namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetProcessStatusesQueryResponse
{
    public List<ProcessStatus> ProcessStatuses { get; set; } = new List<ProcessStatus>();

    public class ProcessStatus
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? IsOutcomeDecision { get; set; }
    }
}
