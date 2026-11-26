using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Api.Models.Users;

public sealed record GetUserRequest
{
    public required string Email { get; set; }
    public required UserType UserType { get; set; }
}