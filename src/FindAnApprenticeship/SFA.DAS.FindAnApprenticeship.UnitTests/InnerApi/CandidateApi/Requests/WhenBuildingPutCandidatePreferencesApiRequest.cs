﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingPutCandidatePreferencesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid candidateId)
    {
        var actual = new PutCandidatePreferencesApiRequest(candidateId, new PutCandidatePreferencesRequestData());

        actual.PutUrl.Should().Be($"api/candidates/{candidateId}/NotificationPreferences/");
    }
}
