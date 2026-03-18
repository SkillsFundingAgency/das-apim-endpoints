using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetStandardFundingResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ApprenticeshipFunding source)
        {
            //Act
            var actual = (GetStandardFundingResponse)source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
    }
}