using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingConfirmIncentiveApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_PatchUrl_Is_Correctly_Build(Guid applicationId, long accountId, DateTime submittedDateTime, string baseUrl)
        {
            var actual = new ConfirmIncentiveApplicationRequest(applicationId, accountId, submittedDateTime)
            {
                BaseUrl = baseUrl
            };

            actual.PatchUrl.Should().Be($"{baseUrl}applications/{applicationId}");
            actual.AccountId.Should().Be(accountId);
            actual.IncentiveApplicationId.Should().Be(applicationId);
            actual.DateSubmitted.Should().Be(submittedDateTime);
        }
    }
}