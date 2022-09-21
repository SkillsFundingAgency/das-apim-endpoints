using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.NationalAchievementRatesLookup
{
    [TestFixture]
    public class NationalAchievementRatesLookupQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_downloadsNationalAchievementRates_ReturnsValidResponse(
            string filePath,
            string content,
            [Frozen] Mock<INationalAchievementRatesPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> downloadService,
            [Frozen] Mock<IZipArchiveHelper> zipArchiveHelper,
            List<NationalAchievementRateCsv> downloadDataAchievementRate,
            List<NationalAchievementRateOverallCsv> downloadDataOverallAchievementRate,
            NationalAchievementRatesLookupQuery query,
            NationalAchievementRatesLookupQueryHandler sut)
        {
            //Arrange
            pageParser.Setup(x => x.GetCurrentDownloadFilePath()).ReturnsAsync(filePath);
            downloadService.Setup(x => x.GetFileStream(filePath)).ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            zipArchiveHelper.Setup(x =>
                x.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(It.IsAny<Stream>(),
                    It.IsAny<string>())).Returns(downloadDataAchievementRate);

            zipArchiveHelper.Setup(x =>
                x.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(It.IsAny<Stream>(),
                    It.IsAny<string>())).Returns(downloadDataOverallAchievementRate);

            //Act
            var result = await sut.Handle(query, new CancellationToken());

            //Assert
            pageParser.Verify(x => x.GetCurrentDownloadFilePath(), Times.Once);
            downloadService.Verify(x => x.GetFileStream(It.IsAny<string>()), Times.Once);

            zipArchiveHelper.Verify(x =>x.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(It.IsAny<Stream>(), It.IsAny<string>()),Times.Once);
            zipArchiveHelper.Verify(x => x.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);

            result.Should().NotBeNull();
        }
    }
}
