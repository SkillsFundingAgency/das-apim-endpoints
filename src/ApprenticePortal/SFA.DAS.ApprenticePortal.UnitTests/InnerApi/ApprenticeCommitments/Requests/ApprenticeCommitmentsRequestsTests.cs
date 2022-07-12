using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using System;

namespace SFA.DAS.ApprenticePortal.UnitTests.InnerApi.ApprenticeAccounts.Requests
{
    public class ApprenticeCommitmentsRequestsTests
    {
        [Test, AutoData]
        public void TestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = "84489dbb-cd85-4c39-847d-2eced378960b";
            var instance = new GetApprenticeApprenticeshipsRequest(new Guid(apprenticeId));

            instance.GetUrl.Should().Be($"apprentices/84489dbb-cd85-4c39-847d-2eced378960b/apprenticeships");
        }
    }
}