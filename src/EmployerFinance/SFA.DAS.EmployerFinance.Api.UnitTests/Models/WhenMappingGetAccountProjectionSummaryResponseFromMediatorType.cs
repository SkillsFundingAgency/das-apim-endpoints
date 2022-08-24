using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Models
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
