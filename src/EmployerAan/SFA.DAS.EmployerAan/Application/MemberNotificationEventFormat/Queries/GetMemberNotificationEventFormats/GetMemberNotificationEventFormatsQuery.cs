﻿using MediatR;

namespace SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;

public record GetMemberNotificationEventFormatsQuery(Guid MemberId) : IRequest<GetMemberNotificationEventFormatsQueryResult>;