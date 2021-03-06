﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetTrainingProgrammeResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(TrainingProgramme source)
        {
            //Arrange
            var actual = (GetTrainingProgrammeResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}