using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Commands.CreateProviderLocation
{
    [TestFixture]
    public class CreateProviderLocationCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokesApiCall(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            CreateProviderLocationCommandHandler sut,
            CreateProviderLocationCommand command)
        {
            await sut.Handle(command, new CancellationToken());
            apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<ProviderLocationCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command)));
        }
    }
}
