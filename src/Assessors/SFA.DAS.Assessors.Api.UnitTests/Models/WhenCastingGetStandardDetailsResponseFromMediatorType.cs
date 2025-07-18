﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.UnitTests.Models
{
    public class WhenCastingGetStandardDetailsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            StandardDetailResponse source)
        {
            var response = (GetStandardDetailsResponse)source;

            response.Should().BeEquivalentTo(source, options =>
                options
                    .Excluding(info => info.Path.EndsWith(".FoundationAppFirstEmpPayment"))
                    .Excluding(info => info.Path.EndsWith(".FoundationAppSecondEmpPayment"))
                    .Excluding(info => info.Path.EndsWith(".FoundationAppThirdEmpPayment")));
        }

        [Test, AutoData]
        public void And_If_StandardDates_Is_Null_Then_Mapped_Field_Is_Null(
            StandardDetailResponse source)
        {
            source.StandardDates = null;
            var response = (GetStandardDetailsResponse)source;

            response.StandardDates.Should().BeNull();
        }

    }
}