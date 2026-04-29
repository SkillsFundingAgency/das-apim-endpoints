using SFA.DAS.Apim.Shared.Interfaces;

public class DeleteApplicationApiRequest : IDeleteApiRequest
{
    public Guid ApplicationId { get; set; }
    public string DeleteUrl => $"/api/applications/{ApplicationId}";
}