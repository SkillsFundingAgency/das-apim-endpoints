using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

public record StatusHistoryModel(OrganisationStatus Status, DateTime AppliedDate);
