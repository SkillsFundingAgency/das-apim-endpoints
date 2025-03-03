public class GetApplicationByIdQueryResponse
{
    public Guid ApplicationId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid OrganisationId { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public int Reference { get; set; }
    public string? QualificationNumber { get; set; }

}