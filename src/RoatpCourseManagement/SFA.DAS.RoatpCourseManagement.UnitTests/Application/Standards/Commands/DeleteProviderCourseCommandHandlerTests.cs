using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class DeleteProviderCourseCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokedDeleteOnApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            DeleteProviderCourseCommandHandler sut,
            DeleteProviderCourseCommand command)
        {
            DeleteProviderCourseRequest expectedRequest = command;
            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(c => c.Delete(It.Is<DeleteProviderCourseRequest>(r => r.UserId == command.UserId && r.Ukprn == command.Ukprn && r.LarsCode == command.LarsCode)));
        }

    }
}
