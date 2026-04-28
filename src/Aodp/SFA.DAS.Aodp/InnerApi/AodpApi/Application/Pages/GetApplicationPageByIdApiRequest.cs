using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationPageByIdApiRequest : IGetApiRequest
{
    public int PageOrder { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageOrder}";
}

