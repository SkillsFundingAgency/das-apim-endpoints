using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string baseUrl, long accountId, Guid applicationId)
        {
            var actual = new GetApplicationRequest(accountId, applicationId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}accounts/{accountId}/applications/{applicationId.ToString()}");
        }
    }
}