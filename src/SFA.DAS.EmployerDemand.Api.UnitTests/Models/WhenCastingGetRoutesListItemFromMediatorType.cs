using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetRoutesListItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Correctly(SharedOuterApi.InnerApi.Responses.GetRoutesListItem source)
        {
            var result = (GetRoutesListItem) source;

            result.Name.Should().Be(source.Name);
            result.Id.Should().Be(source.Id);
        }
    }
}