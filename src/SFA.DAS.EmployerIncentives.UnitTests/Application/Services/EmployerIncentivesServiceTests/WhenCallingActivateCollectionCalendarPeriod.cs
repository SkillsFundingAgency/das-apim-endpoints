using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingActivateCollectionCalendarPeriod
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
           ActivateCollectionCalendarPeriodRequestData request,
           [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
           EmployerIncentivesService sut)
        {
            await sut.ActivateCollectionCalendarPeriod(request);

            client.Verify(x =>
                x.Post<ActivateCollectionCalendarPeriodRequestData>(It.Is<ActivateCollectionCalendarPeriodRequest>(
                    c => ((ActivateCollectionCalendarPeriodRequestData)c.Data).CollectionPeriodNumber == request.CollectionPeriodNumber &&
                    ((ActivateCollectionCalendarPeriodRequestData)c.Data).CollectionPeriodYear == request.CollectionPeriodYear &&
                          c.PostUrl.Equals("collectionCalendar/period/activate"))
                ), Times.Once);
        }
    }
}
