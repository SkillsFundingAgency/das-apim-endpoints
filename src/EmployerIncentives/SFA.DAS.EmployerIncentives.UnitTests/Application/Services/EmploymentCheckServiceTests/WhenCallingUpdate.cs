using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmploymentCheckServiceTests
{
    public class WhenCallingUpdate
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Update_the_emplpyer_check_status(
            UpdateRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            IncentivesEmploymentCheckService service)
        {
            await service.Update(request);

            client.Verify(x =>
                x.Put(It.Is<UpdateEmploymentCheckRequest>(
                    c =>
                        c.PutUrl.Contains(request.CorrelationId.ToString()) && c.Data.Equals(request)//TODO
                )), Times.Once
            );
        }
    }
}