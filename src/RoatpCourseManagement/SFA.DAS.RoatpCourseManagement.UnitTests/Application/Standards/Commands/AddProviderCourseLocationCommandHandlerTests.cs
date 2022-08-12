using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class AddProviderCourseLocationCommandHandlerTests
    {
        [Test, AutoData]
        public async Task Handlers_CallsPostOnApiClient(AddProviderCourseLocationCommand command)
        {

            var apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();

            var response = new ApiResponse<int>(1, HttpStatusCode.Created, string.Empty);
            apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.Is<ProviderCourseLocationCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command && r.PostUrl == $"providers/{command.Ukprn}/courses/{command.LarsCode}/create-providercourselocation"), true)).ReturnsAsync(response);

            var sut = new AddProviderCourseLocationCommandHandler(apiClientMock.Object, Mock.Of<ILogger<AddProviderCourseLocationCommandHandler>>());

            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(a => a.PostWithResponseCode<int>(It.Is<ProviderCourseLocationCreateRequest>(r => r.Ukprn == command.Ukprn && r.LarsCode == command.LarsCode && r.PostUrl == $"providers/{command.Ukprn}/courses/{command.LarsCode}/create-providercourselocation"), true));
        }
    }
}
