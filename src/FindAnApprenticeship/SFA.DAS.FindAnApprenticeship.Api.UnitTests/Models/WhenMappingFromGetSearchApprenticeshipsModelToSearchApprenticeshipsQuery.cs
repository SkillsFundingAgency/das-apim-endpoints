using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromGetSearchApprenticeshipsModelToSearchApprenticeshipsQuery
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetSearchApprenticeshipsModel source, Guid candidateId, List<int> routeIds, List<int> levelIds)
        {
            source.Sort = null;
            source.CandidateId = candidateId.ToString();
            source.LevelIds = levelIds.Select(c=>c.ToString()).ToList();
            source.RouteIds = routeIds.Select(l=>l.ToString()).ToList();
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.CandidateId)
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.LevelIds)
                .Excluding(c => c.Sort));
            actual.SelectedLevelIds.Should().BeEquivalentTo(levelIds);
            actual.SelectedRouteIds.Should().BeEquivalentTo(routeIds);
            actual.CandidateId.Should().Be(candidateId);
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Returned_When_PageSize_And_PageNumber_Are_Zero(GetSearchApprenticeshipsModel source)
        {
            source.PageSize = null;
            source.PageNumber = null;
            source.Sort = null;
            source.CandidateId = null;
            source.LevelIds = null;
            source.RouteIds = null;
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.LevelIds)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.Sort));
            actual.PageSize.Should().Be(Domain.Constants.SearchApprenticeships.DefaultPageSize);
            actual.PageNumber.Should().Be(Domain.Constants.SearchApprenticeships.DefaultPageNumber);
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Returned_When_PageSize_And_PageNumber_Are_Null(GetSearchApprenticeshipsModel source)
        {
            source.PageSize = null;
            source.PageNumber = null;
            source.Sort = null;
            source.CandidateId = null;
            source.LevelIds = null;
            source.RouteIds = null;
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.LevelIds)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.Sort));
            actual.PageSize.Should().Be(Domain.Constants.SearchApprenticeships.DefaultPageSize);
            actual.PageNumber.Should().Be(Domain.Constants.SearchApprenticeships.DefaultPageNumber);
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Returned_When_Sort_Is_Null(GetSearchApprenticeshipsModel source)
        {
            source.PageSize = null;
            source.PageNumber = null;
            source.Sort = null;
            source.CandidateId = null;
            source.LevelIds = null;
            source.RouteIds = null;
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.LevelIds)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.Sort));

            actual.Sort.Should().Be(Domain.Constants.SearchApprenticeships.DefaultSortOrder);
        }
    }
}
