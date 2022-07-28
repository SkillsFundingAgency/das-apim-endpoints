using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
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
    public class DeleteProviderCourseLocationsCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokedDeleteOnApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            DeleteProviderCourseLocationsCommandHandler sut,
            DeleteProviderCourseLocationCommand command)
        {
            DeleteProviderCourseLocationRequest expectedRequest = command;
            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(c => c.Delete(It.Is<DeleteProviderCourseLocationRequest>(r => r.Id == command.Id && r.Ukprn == command.Ukprn && r.LarsCode == command.LarsCode)));
        }

    }
}
