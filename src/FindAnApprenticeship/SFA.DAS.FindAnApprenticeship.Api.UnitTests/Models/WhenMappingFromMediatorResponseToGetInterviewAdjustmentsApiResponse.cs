using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponseToGetInterviewAdjustmentsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetInterviewAdjustmentsQueryResult source)
    {
        var actual = (GetInterviewAdjustmentsApiResponse)source;

        actual.InterviewAdjustmentsDescription.Should().BeEquivalentTo(source.InterviewAdjustmentsDescription);
    }
}
