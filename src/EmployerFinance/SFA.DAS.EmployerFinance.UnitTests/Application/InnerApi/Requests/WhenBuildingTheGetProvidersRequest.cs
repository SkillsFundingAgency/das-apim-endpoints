﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange
            var actual = new GetProvidersRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/providers");
        }
    }
}