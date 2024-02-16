using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetContactDetails
{
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task And_Pledge_Doesnt_Exist_Returns_Null(
            GetContactDetailsQuery getContactDetailsQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetContactDetailsHandler getContactDetailsHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == getContactDetailsQuery.OpportunityId)))
                .ReturnsAsync((Pledge)null);

            var result = await getContactDetailsHandler.Handle(getContactDetailsQuery, CancellationToken.None);

            Assert.That(result, Is.Null);
        }

        [Test, MoqAutoData]
        public async Task And_Pledge_Does_Exist_Returns_NotNull(
            GetContactDetailsQuery getContactDetailsQuery,
            List<ReferenceDataItem> allJobRoles,
            List<ReferenceDataItem> allLevels,
            List<ReferenceDataItem> allSectors,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            [Frozen] Mock<IReferenceDataService> mockReferenceDataService,
            GetContactDetailsHandler getContactDetailsHandler)
        {
            var pledge = _fixture
                .Build<Pledge>()
                .With(x => x.JobRoles, allJobRoles.Take(1).Select(y => y.Id))
                .With(x => x.Levels, allLevels.Take(2).Select(y => y.Id))
                .With(x => x.Sectors, allSectors.Take(3).Select(y => y.Id))
                .Create();

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == getContactDetailsQuery.OpportunityId)))
                .ReturnsAsync(pledge);

            mockReferenceDataService
                .Setup(x => x.GetJobRoles())
                .ReturnsAsync(allJobRoles);
            mockReferenceDataService
                .Setup(x => x.GetLevels())
                .ReturnsAsync(allLevels);
            mockReferenceDataService
                .Setup(x => x.GetSectors())
                .ReturnsAsync(allSectors);

            var result = await getContactDetailsHandler.Handle(getContactDetailsQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            
            CollectionAssert.AreEqual(allJobRoles, result.AllJobRoles);
            Assert.AreEqual(1, result.JobRoles.Count());

            CollectionAssert.AreEqual(allLevels, result.AllLevels);
            Assert.AreEqual(2, result.Levels.Count());

            CollectionAssert.AreEqual(allSectors, result.AllSectors);
            Assert.AreEqual(3, result.Sectors.Count());
        }
    }
}