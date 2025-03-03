using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingSavedSearches
    {
        [Test, MoqAutoData]
        public async Task Then_SavedSearches_Returned_From_Mediator(
            int mockPageSize,
            int mockPageNo,
            int maxApprenticeshipSearchResultCount,
            DateTime lastRunDateTime,
            VacancySort sortOrder,
            GetSavedSearchesQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

            var actual = await sut.Get(lastRunDateTime, mockPageNo, mockPageSize, maxApprenticeshipSearchResultCount, sortOrder, It.IsAny<CancellationToken>()) as ObjectResult;
            var actualValue = actual!.Value as GetSavedSearchesQueryResult;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
                actual.Value.Should().BeOfType<GetSavedSearchesQueryResult>();
                actualValue!.SavedSearchResults.Should().BeEquivalentTo(mockQueryResult.SavedSearchResults);
                actualValue.PageSize.Should().Be(mockQueryResult.PageSize);
                actualValue.PageIndex.Should().Be(mockQueryResult.PageIndex);
                actualValue.TotalCount.Should().Be(mockQueryResult.TotalCount);
                actualValue.TotalPages.Should().Be(mockQueryResult.TotalPages);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
            int mockPageSize,
            int mockPageNo,
            int maxApprenticeshipSearchResultCount,
            DateTime lastRunDateTime,
            VacancySort sortOrder,
            GetSavedSearchesQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

            var actual = await sut.Get(lastRunDateTime, mockPageNo, mockPageSize, maxApprenticeshipSearchResultCount, sortOrder, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
