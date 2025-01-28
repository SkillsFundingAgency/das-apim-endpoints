﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Forms;

public class PublishFormVersionApiRequest : IPutApiRequest
{
    public readonly Guid FormVersionId;

    public PublishFormVersionApiRequest(Guid formVersionId)
    {
        FormVersionId = formVersionId;
        Data = new object(); //Unused
    }

    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/publish";
}