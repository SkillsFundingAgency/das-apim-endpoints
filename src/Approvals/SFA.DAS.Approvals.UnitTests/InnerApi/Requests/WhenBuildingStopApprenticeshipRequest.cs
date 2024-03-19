using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingStopApprenticeshipRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(long apprenticeshipId, StopApprenticeshipRequest.Body body)
        {            
            var actual = new StopApprenticeshipRequest(apprenticeshipId, body);
            
            actual.PostUrl.Should().Be($"api/apprenticeships/{apprenticeshipId}/stop");
        }
    }
}
