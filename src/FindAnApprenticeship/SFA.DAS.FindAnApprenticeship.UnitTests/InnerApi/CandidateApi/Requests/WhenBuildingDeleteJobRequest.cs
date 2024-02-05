﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    public class WhenBuildingDeleteJobRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid jobId)
        {
            var actual = new DeleteJobRequest(applicationId, candidateId, jobId);

            actual.DeleteUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/work-history/{jobId}");
        }
    }
}
