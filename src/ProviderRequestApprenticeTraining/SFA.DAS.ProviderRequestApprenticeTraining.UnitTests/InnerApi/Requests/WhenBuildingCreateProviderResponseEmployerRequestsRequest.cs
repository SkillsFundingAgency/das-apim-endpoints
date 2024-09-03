using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingCreateProviderResponseEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(CreateEmployerResponseEmployerRequestData data)
        {
            var actual = new CreateProviderResponseEmployerRequestRequest(data);

            actual.PostUrl.Should().Be($"api/employerrequest/provider/{data.Ukprn}/acknowledge-requests");
            actual.Data.GetType().GetProperty("EmployerRequestIds")!.GetValue(actual.Data, null).Should().BeEquivalentTo(data.EmployerRequestIds);
            actual.Data.GetType().GetProperty("Ukprn")!.GetValue(actual.Data, null).Should().Be(data.Ukprn);
        }
    }
}
