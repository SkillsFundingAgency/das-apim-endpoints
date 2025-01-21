﻿using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class UnpublishFormVersionApiRequest : IPutApiRequest
{
    public readonly Guid FormVersionId;

    public UnpublishFormVersionApiRequest(Guid formVersionId)
    {
        FormVersionId = formVersionId;
        Data = new object(); //Unused
    }

    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}/unpublish";
}