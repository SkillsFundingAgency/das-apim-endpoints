using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetproviderResponseConfirmationRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(Guid providerResponseId)
        {
            var actual = new GetProviderResponseConfirmationRequest(providerResponseId);

            var expected = $"api/provider-responses/{providerResponseId}/confirmation";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
