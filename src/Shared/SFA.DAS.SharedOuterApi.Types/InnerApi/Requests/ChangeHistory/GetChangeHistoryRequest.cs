using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ChangeHistory;

public class GetChangeHistoryRequest(long apprenticeshipId) : IGetApiRequest
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public string GetUrl => $"api/change-history/{ApprenticeshipId}";
}
