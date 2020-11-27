﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models
{
    public class WhenCastingGetEpaoModelFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            SearchEpaosListItem source)
        {
            var response = (GetEpaoApiModel)source;

            response.Should().BeEquivalentTo(source);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaoApiModel)null;

            response.Should().BeNull();
        }
    }
}