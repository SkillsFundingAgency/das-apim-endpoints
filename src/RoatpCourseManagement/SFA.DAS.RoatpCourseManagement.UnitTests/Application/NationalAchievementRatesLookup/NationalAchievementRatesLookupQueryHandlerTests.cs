﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries;
using SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.IO;
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
            string nationalAchievementRatesDownloadPageUrl = "https://www.gov.uk/government/statistics/national-achievement-rates-tables-{0}-to-{1}";

            pageParser.Setup(x => x.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl)).ReturnsAsync(filePath);
            downloadService.Setup(x => x.GetFileStream(filePath)).ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            zipArchiveHelper.Setup(x =>
                x.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(It.IsAny<Stream>(),
                    It.IsAny<string>())).Returns(downloadDataAchievementRate);

            downloadDataOverallAchievementRate.ForEach(a => a.InstitutionType = "All Institution Type");
            downloadDataOverallAchievementRate.ForEach(a => a.Age = "All Age");

            zipArchiveHelper.Setup(x =>
                x.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(It.IsAny<Stream>(),
                    It.IsAny<string>())).Returns(downloadDataOverallAchievementRate);

            //Act
            var result = await sut.Handle(query, new CancellationToken());

            //Assert
            pageParser.Verify(x => x.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl), Times.Once);
            downloadService.Verify(x => x.GetFileStream(It.IsAny<string>()), Times.Once);

            zipArchiveHelper.Verify(x =>x.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(It.IsAny<Stream>(), It.IsAny<string>()),Times.Once);
            zipArchiveHelper.Verify(x => x.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);

            result.Should().NotBeNull();
            result.NationalAchievementRates.Count.Should().NotBe(0);
            result.OverallAchievementRates.Count.Should().NotBe(0);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Handle_downloadlocationEmpty_ReturnsEmptyResponse(
           [Frozen] Mock<INationalAchievementRatesPageParser> pageParser,
           [Frozen] Mock<IDataDownloadService> downloadService,
           [Frozen] Mock<IZipArchiveHelper> zipArchiveHelper,
           NationalAchievementRatesLookupQuery query,
           NationalAchievementRatesLookupQueryHandler sut)
        {
            //Arrange
            pageParser.Setup(x => x.GetCurrentDownloadFilePath(It.IsAny<string>())).ReturnsAsync(string.Empty);

            
            //Act
            var result = await sut.Handle(query, new CancellationToken());

            //Assert
            pageParser.Verify(x => x.GetCurrentDownloadFilePath(It.IsAny<string>()), Times.Once);
            downloadService.Verify(x => x.GetFileStream(It.IsAny<string>()), Times.Never);

            zipArchiveHelper.Verify(x => x.ExtractModelFromCsvFileZipStream<NationalAchievementRateCsv>(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
            zipArchiveHelper.Verify(x => x.ExtractModelFromCsvFileZipStream<NationalAchievementRateOverallCsv>(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.NationalAchievementRates.Count.Should().Be(0);
            result.OverallAchievementRates.Count.Should().Be(0);
        }
    }
}