using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;
public class EmployerRequestForResponseNotification
{
    public Guid RequestedBy { get; set; }
    public long AccountId { get; set; }
    public List<StandardDetails> Standards { get; set; }

    public static implicit operator EmployerRequestForResponseNotification(InnerApi.Responses.RequestApprenticeTraining.EmployerRequestForResponseNotification source)
    {
        return new EmployerRequestForResponseNotification
        {
            RequestedBy = source.RequestedBy,
            AccountId = source.AccountId,
            Standards = source.Standards.Select(e => (StandardDetails)e).ToList(),
        };
    }
}
public class StandardDetails
{
    public string StandardTitle { get; set; }
    public int StandardLevel { get; set; }

    public static implicit operator StandardDetails(InnerApi.Responses.RequestApprenticeTraining.StandardDetails source)
    {
        return new StandardDetails
        {
            StandardLevel = source.StandardLevel,
            StandardTitle = source.StandardTitle,
        };
    }
}

