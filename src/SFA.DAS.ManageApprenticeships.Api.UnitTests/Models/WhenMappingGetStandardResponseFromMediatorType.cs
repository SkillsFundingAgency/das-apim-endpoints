﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Models
{
    public class WhenMappingGetStandardResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
        {
            //Arrange
            var actual = (GetStandardResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(x=>x.ApprenticeshipFunding)
                .Excluding(x=>x.StandardDates)
                .Excluding(x=>x.TypicalDuration)
            );
            actual.Duration.Should().Be(source.TypicalDuration);
        }
    }
}