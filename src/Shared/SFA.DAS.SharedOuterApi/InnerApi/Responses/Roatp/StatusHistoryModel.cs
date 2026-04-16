using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public record StatusHistoryModel(OrganisationStatus Status, DateTime AppliedDate);
