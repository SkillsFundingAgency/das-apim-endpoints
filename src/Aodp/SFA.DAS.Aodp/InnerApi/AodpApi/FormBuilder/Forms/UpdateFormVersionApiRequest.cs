using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;

public class UpdateFormVersionApiRequest : IPutApiRequest
{
    public Guid FormVersionId { get; set; }
    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}";
}