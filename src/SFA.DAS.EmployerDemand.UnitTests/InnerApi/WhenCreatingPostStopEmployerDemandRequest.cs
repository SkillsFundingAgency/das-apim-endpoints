﻿using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingPostStopEmployerDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid id)
        {
            //Act
            var actual = new PostStopEmployerDemandRequest(id);
            
            //Assert
            actual.PostUrl.Should().Be($"api/demand/{id}/stop");
        }
    }
}