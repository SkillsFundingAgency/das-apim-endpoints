using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerAan.InnerApi.Attendances;

[ExcludeFromCodeCoverage]
public record AttendanceStatus(bool IsAttending);