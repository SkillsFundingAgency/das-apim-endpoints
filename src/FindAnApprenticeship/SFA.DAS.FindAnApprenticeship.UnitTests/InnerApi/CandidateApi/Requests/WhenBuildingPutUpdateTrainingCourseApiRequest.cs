﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;
public class WhenBuildingPutUpdateTrainingCourseApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId,
            Guid trainingCourseId)
    {
        var actual = new PutUpsertTrainingCourseApiRequest(applicationId, candidateId, trainingCourseId, new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData());

        actual.PutUrl.Should().Be($"api/candidates/{candidateId}/applications/{applicationId}/trainingcourses/{trainingCourseId}");
    }
}
