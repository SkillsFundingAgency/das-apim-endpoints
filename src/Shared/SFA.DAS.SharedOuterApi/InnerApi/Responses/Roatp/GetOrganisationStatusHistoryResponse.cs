using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

public record GetOrganisationStatusHistoryResponse(IEnumerable<StatusHistoryModel> StatusHistory);