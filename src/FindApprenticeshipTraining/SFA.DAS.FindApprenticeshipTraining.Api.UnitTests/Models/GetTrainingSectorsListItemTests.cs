using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class GetTrainingSectorsListItemTests
    {
        [Test, AutoData]
        public void WhenMappingGetTrainingSectorsListItem_ThenMapsFieldsAppropriately(
            GetRoutesListItem source
        )
        {
            var response = (GetTrainingSectorsListItem)source;

            response.Id.Should().Be(source.Id);
            response.Route.Should().Be(source.Name);
        }
    }
}