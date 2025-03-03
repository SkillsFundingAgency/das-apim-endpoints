﻿using SFA.DAS.EmployerAan.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.EmployerAan.InnerApi.Attendances;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Attendances.Commands.PutAttendance;
public class PutAttendanceCommandTests
{
    [Test, MoqAutoData]
    public void Constructor_InitialisesAllProperties(Guid calendarEventId, Guid requestedByMemberId, AttendanceStatus attendanceStatus)
    {
        var sut = new PutAttendanceCommand(calendarEventId, requestedByMemberId, attendanceStatus);

        Assert.Multiple(() =>
        {
            Assert.That(sut.CalendarEventId, Is.EqualTo(calendarEventId));
            Assert.That(sut.RequestedByMemberId, Is.EqualTo(requestedByMemberId));
            Assert.That(sut.AttendanceStatus, Is.EqualTo(attendanceStatus));
        });
    }
}
