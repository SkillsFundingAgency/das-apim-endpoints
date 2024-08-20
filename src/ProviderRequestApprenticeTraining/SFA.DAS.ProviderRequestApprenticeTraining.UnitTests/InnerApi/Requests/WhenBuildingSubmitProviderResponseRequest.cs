using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;
using System;
using Azure;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingSubmitproviderResponseRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(SubmitProviderResponseRequestData data)
        {
            var actual = new SubmitProviderResponseRequest(data);
            actual.PostUrl.Should().Be("api/employerrequest/provider/submit-response");
            actual.Data.GetType().GetProperty("EmployerRequestIds")!.GetValue(actual.Data, null).Should().BeEquivalentTo(data.EmployerRequestIds);
            actual.Data.GetType().GetProperty("Ukprn")!.GetValue(actual.Data, null).Should().Be(data.Ukprn);
            actual.Data.GetType().GetProperty("Email")!.GetValue(actual.Data, null).Should().Be(data.Email);
            actual.Data.GetType().GetProperty("Phone")!.GetValue(actual.Data, null).Should().Be(data.Phone);
            actual.Data.GetType().GetProperty("Website")!.GetValue(actual.Data, null).Should().Be(data.Website);
        }
    }
}
