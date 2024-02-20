namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;

public enum UserStatuses
{
    Unknown = 0,
    PendingActivation = 10, // once registered and awaiting activation
    Active = 20, // once activated
    Inactive = 30, // when superseded by a new account (user changed their username)
    Locked = 90, // if locked out for security reasons
    Dormant = 100, // if the account has not been recently used
    PendingDeletion = 999 // when marked for hard deletion following being dormant or if not activated
}