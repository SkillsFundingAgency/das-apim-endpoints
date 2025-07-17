using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostUnfreezePaymentsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Built_And_Data_Passed(Guid apprenticeshipKey)
        {
            var actual = new PostUnfreezePaymentsRequest(apprenticeshipKey);

            actual.PostUrl.Should().Be($"{apprenticeshipKey}/unfreeze");
            actual.ApprenticeshipKey.Should().Be(apprenticeshipKey);
        }
    }
}