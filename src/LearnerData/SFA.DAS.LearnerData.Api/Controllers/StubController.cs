using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Requests;
using System.Diagnostics.CodeAnalysis;

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
    }
}
