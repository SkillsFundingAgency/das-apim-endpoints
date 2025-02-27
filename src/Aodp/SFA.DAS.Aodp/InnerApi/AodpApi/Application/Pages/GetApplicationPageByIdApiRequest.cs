using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationPageByIdApiRequest : IGetApiRequest
{
    public int PageOrder { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/applications/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageOrder}";
}

