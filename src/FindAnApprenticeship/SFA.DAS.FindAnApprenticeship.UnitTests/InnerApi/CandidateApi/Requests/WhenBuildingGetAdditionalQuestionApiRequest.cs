﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetAdditionalQuestionApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid questionId)
    {
        var actual = new GetAdditionalQuestionApiRequest(applicationId, candidateId, questionId);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/additional-question/{questionId}");
    }
}
