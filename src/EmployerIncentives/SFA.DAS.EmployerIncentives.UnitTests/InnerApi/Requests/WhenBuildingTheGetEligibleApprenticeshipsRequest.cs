using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEligibleApprenticeshipsRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long uln, DateTime startDate)
        {
            var actual = new GetEligibleApprenticeshipsRequest(uln, startDate);

            actual.GetUrl.Should().Be($"eligible-apprenticeships/{uln}?startDate={startDate:yyyy-MM-dd}&isApproved=true");
        }
    }
}