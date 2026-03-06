using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Requests;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class StubController : ControllerBase
    {
        [HttpPut("providers/{ukprn}/apprenticeships/{learningKey}")]
        public IActionResult UpdateLearner([FromBody] StubUpdateLearnerRequest payload)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("providers/{ukprn}/apprenticeships/{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteLearner([FromRoute] Guid learningKey)
        {
            return Ok();
        }

        [HttpPost]
        [Route("providers/{ukprn}/apprenticeships")]
        public async Task<IActionResult> CreateLearningRecord([FromRoute] long ukprn, [FromBody] StubUpdateLearnerRequest payload)
        {
            return Ok();
        }

        [HttpPut("providers/{ukprn}/shortCourses/{learningKey}")]
        public IActionResult UpdateShortCoursesLearner([FromBody] StubUpdateShortCourseRequest payload)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("providers/{ukprn}/shortCourses/{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteShortCoursesLearner([FromRoute] Guid learningKey)
        {
            return Ok();
        }

        [HttpPost]
        [Route("providers/{ukprn}/shortCourses")]
        public async Task<IActionResult> CreateShortCoursesLearningRecord([FromRoute] long ukprn, [FromBody] StubUpdateShortCourseRequest payload)
        {
            return Ok();
        }

        [HttpGet("providers/{ukprn}/academicyears/{academicyear}/shortCourses")]
        public async Task<IActionResult> GetShortCourseLearners([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
        {
            return Ok(new GetLearnersResponse());
        }

        [HttpGet("providers/{ukprn}/collectionPeriods/{collectionYear}/{collectionPeriod}/shortCourses")]
        public async Task<IActionResult> GetShortCourseEarnings([FromRoute] string ukprn, [FromRoute] int collectionYear, [FromRoute] int collectionPeriod, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(new GetShortCourseEarningsResponse
            {
                Learners = new[]
                {
                    new ShortCourseLearnerAndEarnings
                    {
                        Course = new ShortCourseCourse
                        {
                            AimSequenceNumber = 1, 
                            CoursePrice = 1000, 
                            Earnings = new []
                            {
                                new ShortCourseEarning { Amount = 300, CollectionMonth = 9, CollectionYear = 2526, Milestone = ShortCourseMilestone.ThirtyPercentLearningComplete },
                                new ShortCourseEarning { Amount = 700, CollectionMonth = 10, CollectionYear = 2526, Milestone = ShortCourseMilestone.LearningComplete }
                            },
                            FundingLineType = "GSO Short Courses - Apprenticeship Units - Levy",
                            Approved = true
                        },
                        LearnerRef = "ABD123",
                        LearningKey = Guid.NewGuid(),
                    }
                },
                Page = page,
                PageSize = pageSize,
                Total = 1,
                TotalPages = 1
            });
        }
    }
}
