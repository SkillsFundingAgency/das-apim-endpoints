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
        public void Then_The_Fields_Are_Mapped(GetSearchApprenticeshipsModel source)
        {
            source.Sort = null;
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.Sort));
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Returned_When_PageSize_And_PageNumber_Are_Zero(GetSearchApprenticeshipsModel source)
        {
            source.PageSize = null;
            source.PageNumber = null;
            source.Sort = null;
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
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
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
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
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.Sort));

            actual.Sort.Should().Be(Domain.Constants.SearchApprenticeships.DefaultSortOrder);
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Returned_When_Sort_Is_Empty(GetSearchApprenticeshipsModel source)
        {
            source.PageSize = null;
            source.PageNumber = null;
            source.Sort = "";
            var actual = (SearchApprenticeshipsQuery)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.RouteIds)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.Sort));

            actual.Sort.Should().Be(Domain.Constants.SearchApprenticeships.DefaultSortOrder);
        }
    }
}
