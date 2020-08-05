using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingCreateIncentiveApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
            CreateIncentiveApplicationRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService sut)
        {
            await sut.CreateIncentiveApplication(request);

            client.Verify(x =>
                x.Post<CreateIncentiveApplicationRequest>(It.Is<PostCreateIncentiveApplicationRequest>(
                    c => (CreateIncentiveApplicationRequest)c.Data == request && 
                        c.PostUrl.Contains("application")
                )), Times.Once);
        }
    }
}