﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.UnitTests.Models
{
    public class WhenCastingGetCourseEpaoListItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetCourseEpaoListItem source)
        {
            var response = (Api.Models.GetCourseEpaoListItem)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}