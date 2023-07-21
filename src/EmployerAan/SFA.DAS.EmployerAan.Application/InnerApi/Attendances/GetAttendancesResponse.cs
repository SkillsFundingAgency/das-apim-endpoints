﻿namespace SFA.DAS.EmployerAan.Application.InnerApi.Attendances;
public record GetAttendancesResponse(List<AttendanceModel> Attendances);

public record AttendanceModel(Guid CalendarEventId, string EventFormat, DateTime EventStartDate, string EventTitle);