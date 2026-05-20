using SFA.DAS.Recruit.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;

public sealed record GetUsersByEmployerAccountIdQueryResult(List<GetUserResponse> Users);