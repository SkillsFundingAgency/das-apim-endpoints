using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_No_Id_Specified_Then_All_Pledges_Returned(
            IEnumerable<Pledge> pledges,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesQueryHandler getPledgesQueryHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery();

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledges())
                .ReturnsAsync(pledges);

            var results = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(pledges, results);
        }
    }
}