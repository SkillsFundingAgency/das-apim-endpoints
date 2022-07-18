using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class BulkDeleteProviderCourseLocationsCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokedDeleteOnApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            BulkDeleteProviderCourseLocationsCommandHandler sut,
            BulkDeleteProviderCourseLocationsCommand command)
        {
            ProviderCourseLocationsBulkDeleteRequest expectedRequest = command;
            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(c => c.Delete(It.Is<ProviderCourseLocationsBulkDeleteRequest>(r => r.DeleteProviderCourseLocationOption == command.DeleteProviderCourseLocationOption && r.Ukprn == command.Ukprn && r.LarsCode == command.LarsCode)));
        }

    }
}
