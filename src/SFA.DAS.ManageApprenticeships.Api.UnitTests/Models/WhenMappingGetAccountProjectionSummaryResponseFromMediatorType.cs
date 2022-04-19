using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Models
{
    public class WhenMappingGetAccountProjectionSummaryResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetAccountProjectionSummaryQueryResult source)
        {
            //Arrange
            var actual = (GetAccountProjectionSummaryResponse)source;

            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}
