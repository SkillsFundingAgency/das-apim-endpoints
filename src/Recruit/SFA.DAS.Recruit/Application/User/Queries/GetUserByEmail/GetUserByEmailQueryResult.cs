using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;

public sealed record GetUserByEmailQueryResult(GetUserResponse User);