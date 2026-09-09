using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class GetTrainingLevelsListItemTests
    {
        [Test, AutoData]
        public void WhenMappingGetTrainingLevelsListItem_ThenMapsFieldsAppropriately(
            GetLevelsListItem source)
        {
            var response = (GetTrainingLevelsListItem)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}