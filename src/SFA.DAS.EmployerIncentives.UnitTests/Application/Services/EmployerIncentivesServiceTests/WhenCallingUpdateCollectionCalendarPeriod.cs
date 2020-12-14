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
    public class WhenCallingUpdateCollectionCalendarPeriod
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
           UpdateCollectionCalendarPeriodRequestData request,
           [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
           EmployerIncentivesService sut)
        {
            await sut.UpdateCollectionCalendarPeriod(request);

            client.Verify(x =>
                x.Patch<UpdateCollectionCalendarPeriodRequestData>(It.Is<UpdateCollectionCalendarPeriodRequest>(
                    c => ((UpdateCollectionCalendarPeriodRequestData)c.Data).CollectionPeriodNumber == request.CollectionPeriodNumber &&
                    ((UpdateCollectionCalendarPeriodRequestData)c.Data).CollectionPeriodYear == request.CollectionPeriodYear &&
                    ((UpdateCollectionCalendarPeriodRequestData)c.Data).Active == request.Active &&
                          c.PatchUrl.Equals("collectionPeriods"))
                ), Times.Once);
        }
    }
}
