using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingExpireEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new ExpireEmployerRequestsRequest();

            actual.PostUrl.Should().Be("api/employerrequest/expire-requests");
        }
    }
}
