﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    public class WhenBuildingPatchApplicationApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId)
        {
            var actual = new PatchApplicationApiRequest(applicationId, candidateId, null);

            actual.PatchUrl.Should().Be($"api/Candidates/{candidateId}/applications/{applicationId}");
        }
    }
}
