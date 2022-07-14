using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long accountId, Guid applicationId)
        {
            var actual = new GetApplicationRequest(accountId, applicationId);

            actual.GetUrl.Should().Be($"accounts/{accountId}/applications/{applicationId.ToString()}");
        }
    }
}