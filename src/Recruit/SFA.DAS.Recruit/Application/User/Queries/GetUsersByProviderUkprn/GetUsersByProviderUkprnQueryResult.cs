using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByProviderUkprn;

public sealed record GetUsersByProviderUkprnQueryResult(List<GetUserResponse> Users);