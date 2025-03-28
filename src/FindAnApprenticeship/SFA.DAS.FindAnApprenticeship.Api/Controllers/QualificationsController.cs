using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateQualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualification;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{

    [ApiController]
    [Route("applications/{applicationId}/qualifications")]
    public class QualificationsController(IMediator mediator, ILogger<QualificationsController> logger) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> GetQualifications([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetQualificationsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId
                });

                if (result is null) return NotFound();

                return Ok((GetQualificationsApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetQualifications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostQualifications([FromRoute] Guid applicationId, [FromBody] PostQualificationsApiRequest request)
        {
            try
            {
                await mediator.Send(new UpdateQualificationsCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    IsComplete = request.IsComplete
                });

                return Created();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostQualifications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet("add/select-type")]
        public async Task<IActionResult> GetAddSelectType([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetQualificationTypesQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId
                });

                return Ok((GetQualificationReferenceTypesApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetAddSelectType : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{qualificationReferenceId}/modify")]
        public async Task<IActionResult> GetModifyQualification([FromRoute] Guid applicationId, [FromRoute]Guid qualificationReferenceId, [FromQuery]Guid candidateId, [FromQuery]Guid? id = null)
        {
            try
            {
                var result = await mediator.Send(new GetAddQualificationQuery
                {
                    QualificationReferenceTypeId = qualificationReferenceId,
                    ApplicationId = applicationId,
                    CandidateId = candidateId,
                    Id = id
                });

                return Ok((GetQualificationReferenceTypeApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetAddQualification : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPost("{qualificationReferenceId}/modify")]
        public async Task<IActionResult> PostModifyQualification([FromRoute] Guid applicationId, [FromRoute]Guid qualificationReferenceId, [FromBody] UpdateApplicationQualificationRequest request)
        {
            try
            {
                await mediator.Send(new UpdateApplicationQualificationCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    Subjects = request.Subjects.Select(c=>new UpdateApplicationQualificationCommand.Subject
                    {
                        ToYear = c.ToYear,
                        Grade = c.Grade,
                        Id = c.Id,
                        Name = c.Name,
                        AdditionalInformation = c.AdditionalInformation,
                        QualificationOrder = c.QualificationOrder,
                        IsPredicted = c.IsPredicted,
                        IsDeleted = c.IsDeleted
                    }).ToList(),
                    QualificationReferenceId = qualificationReferenceId
                });

                return Created();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostAddQualification : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("delete/{qualificationReferenceId}")]
        public async Task<IActionResult> GetDeleteQualifications([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId, [FromQuery] Guid candidateId, [FromQuery] Guid? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var singleQuery = await mediator.Send(new GetDeleteQualificationQuery
                    {
                        ApplicationId = applicationId,
                        CandidateId = candidateId,
                        QualificationReference = qualificationReferenceId,
                        Id = id.Value
                    });

                    if (singleQuery is null) return NotFound();

                    return Ok((GetDeleteQualificationsApiResponse)singleQuery);
                }
                
                var multipleQuery = await mediator.Send(new GetDeleteQualificationsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId,
                    QualificationReference = qualificationReferenceId
                });

                if (multipleQuery == null || multipleQuery.Qualifications.Count == 0) return NotFound();

                return Ok((GetDeleteQualificationsApiResponse)multipleQuery);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetDeleteQualifications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPost("delete/{qualificationReferenceId}")]
        public async Task<IActionResult> PostDeleteQualifications([FromRoute] Guid applicationId, [FromRoute] Guid qualificationReferenceId, PostDeleteQualificationsApiRequest request)
        {
            await mediator.Send(new DeleteQualificationsCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                QualificationReferenceId = qualificationReferenceId,
                Id = request.Id
            });

            return Ok();
        }
    }
}
