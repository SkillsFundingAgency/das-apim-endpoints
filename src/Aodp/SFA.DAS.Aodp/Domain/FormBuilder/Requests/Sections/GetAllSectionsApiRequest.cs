using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Sections;

public class GetAllSectionsApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }


    public string GetUrl => $"/api/forms/{FormVersionId}/sections";
}