using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class AddNationalLocationToProviderCourseLocationsCommandHandlerTests
    {
        [Test, AutoData]
        public async Task Handlers_CallsPostOnApiClient(AddNationalLocationToProviderCourseLocationsCommand command)
        {
            var apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
            var sut = new AddNationalLocationToProviderCourseLocationsCommandHandler(apiClientMock.Object);

            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(a => a.PostWithResponseCode<int>(It.Is<AddNationalLocationToProviderCourseLocationsRequest>(r => r.Ukprn == command.Ukprn && r.LarsCode == command.LarsCode && r.UserId == command.UserId && r.UserDisplayName == command.UserDisplayName && r.PostUrl == $"/providers/{command.Ukprn}/courses/{command.LarsCode}/locations/national"), true));
        }
    }
}
