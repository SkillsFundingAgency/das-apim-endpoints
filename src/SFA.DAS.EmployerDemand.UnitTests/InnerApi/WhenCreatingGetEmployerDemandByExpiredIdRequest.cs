using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetEmployerDemandByExpiredIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid id)
        {
            //Act
            var actual = new GetEmployerDemandByExpiredDemandRequest(id);

            //Assert
            actual.GetUrl.Should().Be($"api/demand?expiredCourseDemandId={id}");
        }
    }
}