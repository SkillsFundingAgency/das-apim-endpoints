using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetEmployerDemandsOlderThan3YearsRequest
    {
        [Test, AutoData]
        public void Then_Sets_Url_Correctly(GetEmployerDemandsOlderThan3YearsRequest request)
        {
            request.GetUrl.Should().Be("api/demand/older-than-3-years");
        }
    }
}