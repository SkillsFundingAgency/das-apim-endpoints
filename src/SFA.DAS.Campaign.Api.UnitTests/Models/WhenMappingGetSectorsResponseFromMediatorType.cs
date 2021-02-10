﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.UnitTests.Models
{
    public class WhenMappingGetSectorsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetSectorsListItem source)
        {
            //Act
            var actual = (GetSectorResponseItem) source;
            
            //Assert
            actual.Route.Should().Be(source.Route);
        }
    }
}