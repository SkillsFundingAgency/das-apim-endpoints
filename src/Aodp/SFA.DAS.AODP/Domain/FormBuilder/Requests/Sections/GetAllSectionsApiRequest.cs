using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;

public class GetAllSectionsApiRequest : IGetApiRequest
{
    private readonly Guid _formVersionId;

    public GetAllSectionsApiRequest(Guid formVersionId)
    {
        _formVersionId = formVersionId;
    }

    public string GetUrl => $"/api/sections/form/{_formVersionId}";
}