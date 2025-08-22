using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StubController : ControllerBase
    {
        [HttpPut]
        [Route("{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult UpdateLearner([FromRoute] Guid learningKey,
            [FromBody] StubUpdateLearnerRequest request)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteLearner([FromRoute] Guid learningKey)
        {
            return Ok();
        }
    }
}
