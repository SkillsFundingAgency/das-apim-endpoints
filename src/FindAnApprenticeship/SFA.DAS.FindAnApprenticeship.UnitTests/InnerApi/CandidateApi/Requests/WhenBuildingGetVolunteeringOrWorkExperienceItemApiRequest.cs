﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingGetVolunteeringOrWorkExperienceItemApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid id,
        Guid candidateId,
        Guid applicationId)
    {
        var actual = new GetWorkHistoryItemApiRequest(applicationId, candidateId, id, WorkHistoryType.WorkExperience);

        actual.GetUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/work-history/{id}?workHistoryType={WorkHistoryType.WorkExperience}");
    }
}
