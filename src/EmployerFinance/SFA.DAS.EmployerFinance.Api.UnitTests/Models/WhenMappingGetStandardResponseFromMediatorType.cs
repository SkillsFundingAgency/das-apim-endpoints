using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Models
{
    public class WhenMappingGetStandardResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
        {
            //Arrange
            var actual = (StandardResponse)source;

            //Assert
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(x => x.LarsCode)
            );
            actual.Id.Should().Be(source.LarsCode);
        }
    }
}