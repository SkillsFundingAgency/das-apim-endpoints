﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Attendances;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers;

public class AttendancesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesApiClient_ReturnsResponse(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        [Greedy] AttendancesController sut,
        GetAttendancesResponse expected,
        Guid memberId, DateTime fromDate, DateTime toDate,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(a => a.GetAttendances(memberId, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Get(memberId, fromDate, toDate, cancellationToken);

        actual.As<OkObjectResult>().Value.Should().Be(expected);
        apiClientMock.Verify(a => a.GetAttendances(memberId, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), cancellationToken));
    }
}
