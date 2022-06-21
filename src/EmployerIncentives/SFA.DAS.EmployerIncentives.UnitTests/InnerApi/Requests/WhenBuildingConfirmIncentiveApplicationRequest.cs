using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingConfirmIncentiveApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_PatchUrl_Is_Correctly_Build(Guid applicationId, long accountId, string submittedByEmail, string submittedByName, DateTime submittedDateTime)
        {

            var actual = new ConfirmIncentiveApplicationRequest
            {
                Data = new ConfirmIncentiveApplicationRequestData(applicationId, accountId, submittedDateTime, submittedByEmail, submittedByName)
            };

            actual.PatchUrl.Should().Be($"applications/{applicationId}");
            actual.Data.AccountId.Should().Be(accountId);
            actual.Data.IncentiveApplicationId.Should().Be(applicationId);
            actual.Data.DateSubmitted.Should().Be(submittedDateTime);
            actual.Data.SubmittedByEmail.Should().Be(submittedByEmail);
            actual.Data.SubmittedByName.Should().Be(submittedByName);
        }
    }
}