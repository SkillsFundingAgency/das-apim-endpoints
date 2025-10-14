using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StubController : ControllerBase
    {
        [HttpPut("providers/{ukprn}/learning/{learningKey}")]
        public IActionResult UpdateLearner([FromBody] StubUpdateLearnerRequest payload)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("providers/{ukprn}/learning/{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteLearner([FromRoute] Guid learningKey)
        {
            return Ok();
        }

        [HttpPost]
        [Route("providers/{ukprn}/learners")]
        public async Task<IActionResult> CreateLearningRecord([FromRoute] long ukprn, [FromBody] CreateLearnerRequest dataRequest)
        {
            return Ok();
        }
    }
}
