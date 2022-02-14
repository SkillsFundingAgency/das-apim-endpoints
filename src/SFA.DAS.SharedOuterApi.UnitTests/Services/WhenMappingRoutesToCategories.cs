using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenMappingRoutesToCategories
    {
        [Test]
        [MoqInlineAutoData("Agriculture, environmental and animal care","2,3,4,15")]
        [MoqInlineAutoData("Business and administration","1,2,4,9,13,15")]
        [MoqInlineAutoData("Care services","1,10")]
        [MoqInlineAutoData("Catering and hospitality","4,7")]
        [MoqInlineAutoData("Construction","3,4,5,7,15")]
        [MoqInlineAutoData("Creative and design","1,4,6,7,8,9,10,15")]
        [MoqInlineAutoData("Digital","4,6,9")]
        [MoqInlineAutoData("Education and childcare","1,13")]
        [MoqInlineAutoData("Engineering and manufacturing","1,3,4,5,7,9,15")]
        [MoqInlineAutoData("Hair and beauty","1,7")]
        [MoqInlineAutoData("Health and science","1,2,3,4,7,8,15")]
        [MoqInlineAutoData("Legal, finance and accounting","4,11,15")]
        [MoqInlineAutoData("Protective services","1,5")]
        [MoqInlineAutoData("Sales, marketing and procurement","1,4,6,7,8,9,15")]
        [MoqInlineAutoData("Transport and logistics","3,4,5,7,8")]
        public void Then_The_Categories_Are_Added_For_Each_Route(
            string route, 
            string expectedCategories,
            CourseService courseService)
        {
            var expectedList = expectedCategories.Split(",").Select(c => Convert.ToInt32(c))
                .Select(x => ((VacancyCategories)x).GetDescription()).ToList();
            
            var actual = courseService.MapRoutesToCategories(new List<string>{route});

            actual.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        [MoqAutoData]
        public void Then_Only_Distinct_Items_Are_Returned(CourseService courseService)
        {
            var expectedList = new List<string>
            {
                VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                VacancyCategories.InformationAndCommunicationTechnology.GetDescription(),
                VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                VacancyCategories.ArtsMediaAndPublishing.GetDescription()
            };
            
            var actual = courseService.MapRoutesToCategories(new List<string>{"Digital","Catering and hospitality"});

            actual.Should().BeEquivalentTo(expectedList);
        }

        [Test, MoqAutoData]
        public void Then_If_List_Is_Null_Then_Null_Returned(CourseService courseService)
        {
            var actual = courseService.MapRoutesToCategories(null);

            actual.Should().BeNull();
        }
    
    }
}