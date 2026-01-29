using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetEpaoResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetEpaosListItem source)
        {
            var actual = (GetEpaoResponse) source;
            
            actual.Id.Should().Be(source.EndPointAssessorOrganisationId);
            actual.Name.Should().Be(source.EndPointAssessorName);
        }
    }
}