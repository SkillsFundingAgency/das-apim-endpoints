using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.ApprenticeAccounts.Requests
{
    public class ApprenticeAccountsRequestsTests
    {
        [Test, AutoData]
        public void TestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = "8e5482b2-1c77-4143-80a5-ee3ddc751075";
            var instance = new GetApprenticeRequest(new Guid(apprenticeId));

            instance.GetUrl.Should().Be($"apprentices/8e5482b2-1c77-4143-80a5-ee3ddc751075");
        }
    }
}
