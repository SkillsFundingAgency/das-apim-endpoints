using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Extensions;
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

        [HttpPut("providers/{ukprn}/apprenticeshipUnits/{learningKey}")]
        public IActionResult UpdateApprenticeshipUnitsLearner([FromBody] StubUpdateApprenticeshipUnitRequest payload)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("providers/{ukprn}/apprenticeshipUnits/{learningKey}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteApprenticeshipUnitsLearner([FromRoute] Guid learningKey)
        {
            return Ok();
        }

        [HttpPost]
        [Route("providers/{ukprn}/apprenticeshipUnits")]
        public async Task<IActionResult> CreateApprenticeshipUnitsLearningRecord([FromRoute] long ukprn, [FromBody] StubUpdateApprenticeshipUnitRequest payload)
        {
            return Ok();
        }

        [HttpGet("providers/{ukprn}/academicyears/{academicyear}/apprenticeshipUnits")]
        public async Task<IActionResult> GetApprenticeshipUnitLearners([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
        {
            return Ok(new GetLearnersResponse());
        }
    }
}
