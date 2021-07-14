using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.ReferenceDataService
{
    [TestFixture]
    public class ReferenceDataServiceTests
    {
        private LevyTransferMatching.Application.Services.ReferenceDataService _referenceDataService;
        private Mock<ICacheStorageService> _cacheStorageService;
        private Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> _apiClient;

        private List<ReferenceDataItem> _sectors;
        private List<ReferenceDataItem> _jobRoles;
        private List<ReferenceDataItem> _levels;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            _cacheStorageService = new Mock<ICacheStorageService>();

            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();

            _apiClient.Setup(x => x.GetAll<ReferenceDataItem>(It.IsAny<GetSectorsRequest>()))
                .ReturnsAsync(() => _sectors);

            _apiClient.Setup(x => x.GetAll<ReferenceDataItem>(It.IsAny<GetLevelsRequest>()))
                .ReturnsAsync(() => _levels);

            _apiClient.Setup(x => x.GetAll<ReferenceDataItem>(It.IsAny<GetJobRolesRequest>()))
                .ReturnsAsync(() => _jobRoles);

            _cacheStorageService
                .Setup(x => x.RetrieveFromCache<List<ReferenceDataItem>>("ReferenceData.reference/levels"))
                .ReturnsAsync(() => null);

            _cacheStorageService
                .Setup(x => x.RetrieveFromCache<List<ReferenceDataItem>>("ReferenceData.reference/sectors"))
                .ReturnsAsync(() => null);

            _cacheStorageService
                .Setup(x => x.RetrieveFromCache<List<ReferenceDataItem>>("ReferenceData.reference/jobRoles"))
                .ReturnsAsync(() => null);

            _referenceDataService = new LevyTransferMatching.Application.Services.ReferenceDataService(_cacheStorageService.Object, _apiClient.Object);
        }

        [Test]
        public async Task GetSectors_Retrieves_Sectors()
        {
            var result = await _referenceDataService.GetSectors();
            Assert.AreEqual(_sectors, result);
        }

        [Test]
        public async Task GetSectors_Retrieves_Levels()
        {
            var result = await _referenceDataService.GetLevels();
            Assert.AreEqual(_levels, result);
        }

        [Test]
        public async Task GetSectors_Retrieves_JobRoles()
        {
            var result = await _referenceDataService.GetJobRoles();
            Assert.AreEqual(_jobRoles, result);
        }

        [Test]
        public async Task GetSectors_Retrieves_Sectors_From_Cache()
        {
            SetupCache(_sectors, new GetSectorsRequest());

            var result = await _referenceDataService.GetSectors();
            Assert.AreEqual(_sectors, result);
        }

        [Test]
        public async Task GetSectors_Retrieves_Levels_From_Cache()
        {
            SetupCache(_levels, new GetLevelsRequest());

            var result = await _referenceDataService.GetLevels();
            Assert.AreEqual(_levels, result);
        }

        [Test]
        public async Task GetSectors_Retrieves_JobRoles_From_Cache()
        {
            SetupCache(_jobRoles, new GetJobRolesRequest());

            var result = await _referenceDataService.GetJobRoles();
            Assert.AreEqual(_jobRoles, result);
        }

        private void SetupCache(List<ReferenceDataItem> data, IGetAllApiRequest apiRequest)
        {
            _cacheStorageService
                .Setup(x => x.RetrieveFromCache<List<ReferenceDataItem>>($"ReferenceData.{apiRequest.GetAllUrl}"))
                .ReturnsAsync(data);

            _apiClient.Setup(x => x.GetAll<ReferenceDataItem>(It.Is<IGetAllApiRequest>(r => r.GetType() == apiRequest.GetType())))
                .ReturnsAsync(() => null);
        }
    }
}
