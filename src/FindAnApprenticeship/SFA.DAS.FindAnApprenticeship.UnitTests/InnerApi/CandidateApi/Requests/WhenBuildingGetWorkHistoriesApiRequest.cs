﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingGetWorkHistoriesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetWorkHistoriesApiRequest(applicationId, candidateId, WorkHistoryType.Job);

        actual.GetUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/work-history?workHistoryType=Job");
    }
}