using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetLocationInformation
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Api_Called_And_Location_Information_Returned(
            GetLocationInformationQuery getLocationInformationQuery,
            GetLocationInformationQueryHandler getLocationInformationQueryHandler)
        {
            var result = await getLocationInformationQueryHandler.Handle(getLocationInformationQuery, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
