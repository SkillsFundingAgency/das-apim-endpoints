using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies;

public class GetVacanciesQuery: IRequest<GetVacanciesQueryResult>
{
    public AccountIdentifier AccountIdentifier { get ; set ; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string AccountPublicHashedId { get; set; }
    public string EmployerName { get; set; }
    public List<string> AdditionalDataSources { get; set; }
    public uint? DistanceInMiles { get ; set ; }
    public bool? ExcludeNational { get; set; }
    public double? Lat { get ; set ; }
    public double? Lon { get ; set ; }
    public bool? NationWideOnly { get ; set ; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public uint? PostedInLastNumberOfDays { get ; set ; }
    public List<string> Routes { get ; set ; }
    public string Sort { get ; set ; }
    public List<int> StandardLarsCode { get ; set ; }
    public int? Ukprn { get; set; }
}