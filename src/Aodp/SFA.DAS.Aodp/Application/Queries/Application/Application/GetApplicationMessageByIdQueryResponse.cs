namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessageByIdQueryResponse
{
    public Guid MessageId { get; set; }
    public Guid ApplicationId { get; set; }
    public string MessageText { get; set; }
    public string MessageType { get; set; }
    public string MessageHeader { get; set; }
    public bool SharedWithDfe { get; set; }
    public bool SharedWithOfqual { get; set; }
    public bool SharedWithSkillsEngland { get; set; }
    public bool SharedWithAwardingOrganisation { get; set; }
    public string SentByName { get; set; }
    public string SentByEmail { get; set; }
    public DateTime SentAt { get; set; }
}