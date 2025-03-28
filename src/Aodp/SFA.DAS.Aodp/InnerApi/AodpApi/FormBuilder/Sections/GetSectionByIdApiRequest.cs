﻿using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Sections;

public class GetSectionByIdApiRequest : IGetApiRequest
{
    private readonly Guid _sectionId;
    private readonly Guid _formVersionId;

    public GetSectionByIdApiRequest(Guid sectionId, Guid formVersionId)
    {
        _sectionId = sectionId;
        _formVersionId = formVersionId;
    }

    public string GetUrl => $"/api/forms/{_formVersionId}/sections/{_sectionId}";
}