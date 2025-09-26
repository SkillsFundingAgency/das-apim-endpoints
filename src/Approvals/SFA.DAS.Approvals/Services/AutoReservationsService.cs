using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Validation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services;

public interface IAutoReservationsService
{
    public Task<Guid> CreateReservation(AutoReservation command);
    public Task DeleteReservation(Guid id);
}

public class AutoReservationsService : IAutoReservationsService
{
    private readonly IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
    private readonly ILogger<AutoReservationsService> _logger;

    public AutoReservationsService(IReservationApiClient<ReservationApiConfiguration> reservationsApiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ILogger<AutoReservationsService> logger)
    {
        _reservationsApiClient = reservationsApiClient;
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<Guid> CreateReservation(AutoReservation command)
    {
        var id = Guid.NewGuid();
        Guid.TryParse(command.UserInfo.UserId, out var userId);

        _logger.LogInformation("Getting Account Legal Entity for {0}", command.AccountLegalEntityId);
        var ale = await _apiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(command.AccountLegalEntityId));
        if (ale == null)
        {
            throw new ApplicationException("When creating an auto reservation, the AccountLegalEntity was not found");
        }

        if (string.IsNullOrWhiteSpace(command.CourseCode))
        {
            throw CreateApiModelException("CourseCode", "Course must be correctly assigned");
        }

        _logger.LogInformation("Creating Reservation with id {0}", id);
        var request = new CreateReservationRequest
        {
            AccountId = command.AccountId,
            AccountLegalEntityId = command.AccountLegalEntityId,
            AccountLegalEntityName = ale.LegalEntityName,
            CourseId = command.CourseCode,
            IsLevyAccount = false,
            Id = id,
            ProviderId = null,
            StartDate = command.StartDate,
            TransferSenderAccountId = null,
            UserId = userId
        };

        var post = new PostCreateReservationRequest(request);
        var response = await _reservationsApiClient.PostWithResponseCode<CreateReservationResponse>(post);

        if (response.Body != null)
        {
            _logger.LogInformation("Reservation id {0} was created", id);
            return response.Body.Id;
        }

        var exception = ConvertToDomainOrApplicationException(response.ErrorContent);
        throw exception;
    }

    public Task DeleteReservation(Guid id)
    {
        return _reservationsApiClient.Delete(new DeleteReservationRequest(id));
    }

    private Exception ConvertToDomainOrApplicationException(string responseErrorContent)
    {
        _logger.LogInformation("Creating of reservation failed with this content {0}", responseErrorContent);

        if (responseErrorContent.Contains("CourseId", StringComparison.InvariantCultureIgnoreCase))
        {
            return CreateApiModelException("ReservationId", "Funding is not available for this course on this start date"); ;
        }

        var error = JsonConvert.DeserializeObject<ReservationsStartDateErrorResponse>(responseErrorContent);

        if (error == null || error.StartDate == null || !error.StartDate.Any())
        {
            return CreateApiModelException("ReservationId", "Unexpected error when creating an auto reservation, please try later"); ;
        }

        var startDateErrorMessage = error.StartDate.FirstOrDefault();
        if (!string.IsNullOrEmpty(startDateErrorMessage))
        {
            return CreateApiModelException("StartDate", startDateErrorMessage);
        }
        return CreateApiModelException("ReservationId", "Unexpected error when reading an auto reservation error, please try later"); ;
    }

    private Exception CreateApiModelException(string fieldName, string message)
    {
        _logger.LogInformation("Creation of reservation failed for field {0} with message {1}", fieldName, message);

        var errorDetail = new ErrorDetail(fieldName, message);
        var errorResponse = new ErrorResponse(new List<ErrorDetail> { errorDetail });

        var content = JsonConvert.SerializeObject(errorResponse);

        return new DomainApimException(content);
    }
}

public class AutoReservation
{
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string CourseCode { get; set; }
    public DateTime? StartDate { get; set; }
    public UserInfo UserInfo { get; set; }
}