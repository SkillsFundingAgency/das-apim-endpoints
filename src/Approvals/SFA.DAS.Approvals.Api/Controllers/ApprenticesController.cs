using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm;
using SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ConfirmEditApprenticeship;
using SFA.DAS.Approvals.Application.Apprentices.Queries;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.ApprenticeshipDetails;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetEditApprenticeshipCourse;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ApprenticeData;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.Inform;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates;
using SFA.DAS.Approvals.Exceptions;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ApprenticesController(
    ILogger<ApprenticesController> logger,
    IMediator mediator, 
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetApprenticeQuery { ApprenticeId = id });
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting Apprentice data {id}", id);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/inform")]
    public async Task<IActionResult> ChangeEmployerInform(long providerId, long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetInformQuery
                { ApprenticeshipId = apprenticeshipId, ProviderId = providerId });

            if (result == null)
            {
                return NotFound();
            }

            var response = new GetInformResponse
            {
                LegalEntityName = result.LegalEntityName,
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting change employer - inform, apprenticeship {id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/confirm-employer")]
    public async Task<IActionResult> ChangeEmployerConfirmEmployer(long providerId, long apprenticeshipId, [FromQuery] long accountLegalEntityId)
    {
        try
        {
            var result = await mediator.Send(new GetConfirmEmployerQuery
                { ApprenticeshipId = apprenticeshipId, ProviderId = providerId, AccountLegalEntityId = accountLegalEntityId });

            if (result == null)
            {
                return NotFound();
            }

            var response = new GetConfirmEmployerResponse
            {
                LegalEntityName = result.LegalEntityName,
                AccountLegalEntityName = result.AccountLegalEntityName,
                AccountName = result.AccountName,
                IsFlexiJobAgency = result.IsFlexiJobAgency,
                DeliveryModel = result.DeliveryModel
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting change employer - confirm employer, apprenticeship {id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/apprenticeship-data")]
    public async Task<IActionResult> GetChangeEmployerApprenticeshipData(long providerId, long apprenticeshipId, [FromQuery] long accountLegalEntityId)
    {
        try
        {
            var result = await mediator.Send(new GetChangeOfEmployerApprenticeDataQuery
                { ApprenticeshipId = apprenticeshipId, AccountLegalEntityId = accountLegalEntityId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting change employer - apprentice data , apprenticeship {id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/confirm")]
    public async Task<IActionResult> ChangeEmployerConfirm(long providerId, long apprenticeshipId, [FromBody] CreateChangeOfEmployerRequest request)
    {
        try
        {
            await mediator.Send(new CreateChangeOfEmployerCommand
            {
                ApprenticeshipId = apprenticeshipId,
                ProviderId = providerId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                Price = request.Price,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                EmploymentEndDate = request.EmploymentEndDate,
                EmploymentPrice = request.EmploymentPrice,
                DeliveryModel = request.DeliveryModel,
                HasOverlappingTrainingDates = request.HasOverlappingTrainingDates,
                UserInfo = request.UserInfo
            });

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting change employer - inform, apprenticeship {id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/select-delivery-model")]
    public async Task<IActionResult> ChangeEmployerSelectDeliveryModel(long providerId, long apprenticeshipId, [FromQuery] long accountLegalEntityId)
    {
        try
        {
            var result = await mediator.Send(new GetSelectDeliveryModelQuery
                { ApprenticeshipId = apprenticeshipId, ProviderId = providerId, AccountLegalEntityId = accountLegalEntityId });

            if (result == null)
            {
                return NotFound();
            }

            var response = new GetSelectDeliveryModelResponse
            {
                LegalEntityName = result.LegalEntityName,
                DeliveryModels = result.DeliveryModels,
                Status = result.Status
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting change employer - select-delivery-model, apprenticeship {id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/edit")]
    [Route("/employer/{accountId}/apprentices/{apprenticeshipId}/edit")]
    public async Task<IActionResult> EditApprenticeship(long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetEditApprenticeshipQuery { ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok((GetEditApprenticeshipResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error in GetApprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}")]
    public async Task<IActionResult> EditApprenticeshipProvider(long providerId, long apprenticeshipId,
        [FromBody] EditApprenticeshipRequest request)
    {
        return await EditApprenticeshipInternal(apprenticeshipId, providerId, request.EmployerAccountId, request);
    }

    [HttpPut]
    [Route("/employer/{accountId}/apprentices/{apprenticeshipId}")]
    public async Task<IActionResult> EditApprenticeshipEmployer(long accountId, long apprenticeshipId,
        [FromBody] EditApprenticeshipRequest request)
    {
        logger.LogInformation("=== APIM CONTROLLER: EditApprenticeshipEmployer called ===");
        logger.LogInformation("AccountId: {AccountId}, ApprenticeshipId: {ApprenticeshipId}", accountId, apprenticeshipId);
        
        // Log all incoming headers
        logger.LogInformation("=== INCOMING REQUEST HEADERS ===");
        if (Request?.Headers != null)
        {
            foreach (var header in Request.Headers)
            {
                logger.LogInformation("Header: {HeaderKey} = {HeaderValue}", header.Key, header.Value);
            }
        }
        else
        {
            logger.LogInformation("Request or Headers is null (unit test context)");
        }
        
        // Log authentication context
        if (User?.Identity?.IsAuthenticated == true)
        {
            logger.LogInformation("User is authenticated: {UserName}", User.Identity.Name);
            logger.LogInformation("User claims:");
            foreach (var claim in User.Claims)
            {
                logger.LogInformation("Claim: {ClaimType} = {ClaimValue}", claim.Type, claim.Value);
            }
        }
        else
        {
            logger.LogWarning("User is NOT authenticated or User is null!");
        }
        
        return await EditApprenticeshipInternal(apprenticeshipId, request.ProviderId, accountId, request);
    }

    private async Task<IActionResult> EditApprenticeshipInternal(
        long apprenticeshipId, 
        long? providerId, 
        long? employerAccountId, 
        EditApprenticeshipRequest request)
    {
        var command = new EditApprenticeshipCommand
        {
            ApprenticeshipId = apprenticeshipId,
            EmployerAccountId = employerAccountId,
            ProviderId = providerId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            ULN = request.ULN,
            CourseCode = request.CourseCode,
            Version = request.Version,
            Option = request.Option,
            Cost = request.Cost,
            EmployerReference = request.EmployerReference,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DeliveryModel = request.DeliveryModel,
            ProviderReference = request.ProviderReference,
            EmploymentEndDate = request.EmploymentEndDate,
            EmploymentPrice = request.EmploymentPrice
        };

        var result = await mediator.Send(command);

        return Ok(new EditApprenticeshipResponse
        {
            ApprenticeshipId = result.ApprenticeshipId,
            HasOptions = result.HasOptions,
            Version = result.Version,
            CourseOrStartDateChange =  result.CourseOrStartDateChanged
        });
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/edit/delivery-model")]
    [Route("/employer/{accountId}/apprentices/{apprenticeshipId}/edit/delivery-model")]
    public async Task<IActionResult> EditApprenticeshipDeliveryModel(long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetEditApprenticeshipDeliveryModelQuery { ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok((GetEditApprenticeshipDeliveryModelResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error in GetApprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }


    [HttpGet]
    [Route("/employer/{providerId}/apprentices/{apprenticeshipId}/edit/select-course")]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/edit/select-course")]
    public async Task<IActionResult> GetEditApprenticeshipCourse(long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetEditApprenticeshipCourseQuery { ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok((GetEditApprenticeshipCourseResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error in GetEditApprenticeshipCourse apprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/employer/{providerId}/apprentices/{apprenticeshipId}/changes/review")]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/changes/review")]
    public async Task<IActionResult> GetReviewApprenticeshipUpdates(long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetReviewApprenticeshipUpdatesQuery { ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok((GetReviewApprenticeshipUpdatesResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error in GetReviewApprenticeshipUpdates apprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }


    [HttpGet]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/details")]
    [Route("/employer/{accountId}/apprentices/{apprenticeshipId}/details")]
    public async Task<IActionResult> ApprenticeshipDetails(long apprenticeshipId)
    {
        try
        {
            var result = await mediator.Send(new GetApprenticeshipDetailsQuery { ApprenticeshipId = apprenticeshipId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok((GetApprenticeshipDetailsResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error in GetApprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/provider/{providerId}/apprenticeships/{apprenticeshipId}/details")]
    [Route("/employer/{accountId}/apprenticeships/{apprenticeshipId}/details")]
    public async Task<IActionResult> ManageApprenticeshipDetails(long apprenticeshipId)
    {
        logger.LogInformation("GET ManageApprenticeshipDetails starting for apprenticeshipId {Id}", apprenticeshipId);

        try
        {
            var result = await mediator.Send(new GetManageApprenticeshipDetailsQuery
            {
                ApprenticeshipId = apprenticeshipId
            });

            if (result == null)
            {
                logger.LogWarning("GET ManageApprenticeshipDetails apprenticeshipId with the id not found {Id}", apprenticeshipId);

                return NotFound();
            }

            var response = mapper.Map<GetManageApprenticeshipDetailsResponse>(result);

            logger.LogInformation("GET ManageApprenticeshipDetails returning OK for apprenticeshipId: {Id}", apprenticeshipId);

            return Ok(response);
        }
        catch (ResourceNotFoundException notFoundException)
        {
            logger.LogError(notFoundException, "The apprenticeship with the Id {Id} was not found.", apprenticeshipId);
            return NotFound();
        }
        catch (UnauthorizedAccessException unauthorizedAccessException)
        {
            logger.LogError(unauthorizedAccessException, "Permission denied when accessing apprenticeshipId {Id}", apprenticeshipId);
            return Unauthorized();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "GET ManageApprenticeshipDetails exception caught for apprenticeshipId {Id}", apprenticeshipId);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/edit/confirm")]
    public async Task<IActionResult> ConfirmEditApprenticeshipProvider(
        [FromRoute] long providerId, 
        [FromRoute] long apprenticeshipId, 
        [FromBody] ConfirmEditApprenticeshipRequest request)
    {
        return await ConfirmEditApprenticeshipInternal(apprenticeshipId, providerId, null, request);
    }

    [HttpPost]
    [Route("/employer/{accountId}/apprentices/{apprenticeshipId}/edit/confirm")]
    public async Task<IActionResult> ConfirmEditApprenticeshipEmployer(
        [FromRoute] long accountId, 
        [FromRoute] long apprenticeshipId, 
        [FromBody] ConfirmEditApprenticeshipRequest request)
    {
        return await ConfirmEditApprenticeshipInternal(apprenticeshipId, null, accountId, request);
    }

    [HttpPost]
    [Route("/provider/{providerId}/apprenticeships/download")]
    public async Task<IActionResult> GetApprenticeshipsCSV(long providerId, [FromBody] PostApprenticeshipsCSVRequest request)
    {
        try
        {
            logger.LogInformation("GetApprenticeshipsCSV starting for providerId {Id}", providerId);

            var query = new GetApprenticeshipsCSVQuery
            {
                ProviderId = providerId,
                SearchTerm = request.SearchTerm,
                EmployerName = request.EmployerName,
                CourseName = request.CourseName,
                Status = request.Status,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Alert = request.Alert,
                ApprenticeConfirmationStatus = request.ApprenticeConfirmationStatus,
                DeliveryModel = request.DeliveryModel
            };

            var apprenticesData = await mediator.Send(query);

            if (apprenticesData == null)
            {
                return NotFound();
            }

            var response = mapper.Map<PostApprenticeshipsCSVResponse>(apprenticesData);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetApprenticeshipsCSV for provider Id: {providerId}", providerId);
            return BadRequest();
        }
    }
    
    private async Task<IActionResult> ConfirmEditApprenticeshipInternal(
        long apprenticeshipId, 
        long? providerId, 
        long? accountId, 
        ConfirmEditApprenticeshipRequest request)
    {
        try
        {
            var command = new ConfirmEditApprenticeshipCommand
            {
                ApprenticeshipId = apprenticeshipId,
                AccountId = accountId,
                ProviderId = providerId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Cost = request.Cost,
                ProviderReference = request.ProviderReference,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DeliveryModel = request.DeliveryModel,
                EmploymentEndDate = request.EmploymentEndDate,
                EmploymentPrice = request.EmploymentPrice,
                CourseCode = request.CourseCode,
                Version = request.Version,
                Option = request.Option,
                UserInfo = request.UserInfo,
                EmployerReference = request.EmployerReference,
            };

            var result = await mediator.Send(command);

            return Ok(new ConfirmEditApprenticeshipResponse
            {
                ApprenticeshipId = result.ApprenticeshipId,
                NeedReapproval = result.NeedReapproval
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error confirming edit apprenticeship {apprenticeshipId}");
            return BadRequest();
        }
    }
}