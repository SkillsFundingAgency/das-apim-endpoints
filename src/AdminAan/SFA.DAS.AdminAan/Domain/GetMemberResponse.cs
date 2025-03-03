﻿namespace SFA.DAS.AdminAan.Domain;

public record GetMemberResponse(Guid MemberId, string Email, string FirstName, string LastName, string? OrganisationName, int? RegionId, MemberUserType UserType, bool? IsRegionalChair, DateTime JoinedDate, string FullName, bool ReceiveNotifications, ApprenticeModel? Apprentice, EmployerModel? Employer);

public record ApprenticeModel(Guid ApprenticeId);

public record EmployerModel(long AccountId, Guid UserRef);
