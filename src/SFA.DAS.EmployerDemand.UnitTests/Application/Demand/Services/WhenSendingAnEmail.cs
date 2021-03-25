using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Services
{
    public class WhenSendingAnEmail
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Correct_Message_To_Notification_Service(
            CreateDemandConfirmationEmail email,
            [Frozen] Mock<IMessageSession> mockMessageSession,
            NotificationService service)
        {
            await service.Send(email);

            mockMessageSession.Verify(session => session.Send(email, It.IsAny<SendOptions>()));
        }
    }
}