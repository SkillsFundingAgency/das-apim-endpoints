namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public record ProviderAllowedCourseModel(string LarsCode, string Title, int Level, DateTime? LastDateStarts, bool IsStartRestricted, bool IsActive);
