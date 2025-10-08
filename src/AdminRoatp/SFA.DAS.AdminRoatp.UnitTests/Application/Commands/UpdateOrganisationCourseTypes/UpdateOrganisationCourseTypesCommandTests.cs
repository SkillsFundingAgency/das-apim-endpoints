using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpdateOrganisationCourseTypes;
public class UpdateOrganisationCourseTypesCommandTests
{
    [Test, MoqAutoData]

    public void UpdateCourseTypesRequest_ReturnsCorrectGetUrl(
        UpdateOrganisationCourseTypesCommand command,
        UpdateCourseTypesModel model)
    {
        var expectedUrl = $"/organisations/{command.ukprn}/allowed-short-courses";

        UpdateCourseTypesRequest apiRequest = new(command.ukprn, model);

        apiRequest.PutUrl.Should().Be(expectedUrl);
    }

    public void UpdateCourseTypesRequest_ReturnsCorrectDataModell(
        UpdateOrganisationCourseTypesCommand command,
        UpdateCourseTypesModel model)
    {
        UpdateCourseTypesRequest apiRequest = new(command.ukprn, model);

        apiRequest.data.Should().BeEquivalentTo(model);
    }
}