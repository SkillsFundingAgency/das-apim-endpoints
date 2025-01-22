using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Sections;

public class DeleteSectionApiRequest : IDeleteApiRequest
{
    private readonly Guid _sectionId;

    public DeleteSectionApiRequest(Guid sectionId)
    {
        _sectionId = sectionId;
    }

    public string DeleteUrl => $"/api/sections/{_sectionId}";
}