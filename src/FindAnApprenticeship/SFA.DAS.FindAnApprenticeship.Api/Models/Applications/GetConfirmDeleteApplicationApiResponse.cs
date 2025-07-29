using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetConfirmDeleteApplicationApiResponse(ConfirmDeleteApplicationQueryResult result)
{
    public Address? Address => result.Address;
    public Guid ApplicationId => result.ApplicationId;
    public DateTime? ApplicationStartDate => result.ApplicationStartDate;
    public ApprenticeshipTypes? ApprenticeshipType => result.ApprenticeshipType;
    public AvailableWhere? EmployerLocationOption => result.EmployerLocationOption;
    public string EmployerName => result.EmployerName;
    public List<Address>? OtherAddresses => result.OtherAddresses;
    public DateTime? VacancyClosedDate => result.VacancyClosedDate;
    public DateTime? VacancyClosingDate => result.VacancyClosingDate;
    public string VacancyTitle => result.VacancyTitle;
}