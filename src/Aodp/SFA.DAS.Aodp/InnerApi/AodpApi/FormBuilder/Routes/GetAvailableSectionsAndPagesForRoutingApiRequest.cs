using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;

public class GetAvailableSectionsAndPagesForRoutingApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }


    public string GetUrl => $"/api/routes/forms/{FormVersionId}/available-sections";
}
