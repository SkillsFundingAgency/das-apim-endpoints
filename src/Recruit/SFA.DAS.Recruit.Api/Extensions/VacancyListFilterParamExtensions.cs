using System.Collections.Generic;
using SFA.DAS.Common.Domain.Models;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.GraphQL;

namespace SFA.DAS.Recruit.Api.Extensions;

public static class VacancyListFilterParamExtensions
{
    public static VacancyEntityFilterInput Build(this VacancyListFilterParams filterParams, int? ukprn = null, long? accountId = null)
    {
        List<VacancyEntityFilterInput> andFilters = [new()
        {
            ClosedDate = new DateTimeOperationFilterInput { Eq = null },
        }];
        
        if (ukprn is not null)
        {
            andFilters.Add(new VacancyEntityFilterInput { // Ukprn = 123 AND OwnerType = Provider
                Ukprn = new IntOperationFilterInput { Eq = ukprn },
                OwnerType = new NullableOfOwnerTypeOperationFilterInput { Eq = OwnerType.Provider }
            });
        }
        else if (accountId is not null)
        {
            andFilters.Add(new VacancyEntityFilterInput
            {
                Or = [ // (OwnerType = Employer OR (OwnerType = Provider AND Status = Review))
                    new VacancyEntityFilterInput
                    {
                        OwnerType = new NullableOfOwnerTypeOperationFilterInput { Eq = OwnerType.Employer }
                    },
                    new VacancyEntityFilterInput
                    {
                        OwnerType = new NullableOfOwnerTypeOperationFilterInput { Eq = OwnerType.Provider },
                        Status = new VacancyStatusOperationFilterInput { Eq = GraphQL.VacancyStatus.Review }
                    },
                ]
            });
        }
        
        if (string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            return new VacancyEntityFilterInput { And = andFilters };
        }
        
        // this will permit searching by 10000 vs VAC10000
        if (VacancyReference.TryParse(filterParams.SearchTerm, out var vacancyReference) && vacancyReference != VacancyReference.None)
        {
            andFilters.Add(new VacancyEntityFilterInput
            {
                VacancyReference = new LongOperationFilterInput { Eq = vacancyReference.Value }
            });
        }
        else
        {
            andFilters.Add(new VacancyEntityFilterInput
            {
                Or = [ // (LegalEntityName like '%searchTerm%' OR Title like '%searchTerm%')
                    new VacancyEntityFilterInput
                    {
                        LegalEntityName = new StringOperationFilterInput
                        {
                            Contains = filterParams.SearchTerm
                        }
                    },
                    new VacancyEntityFilterInput
                    {
                        Title = new StringOperationFilterInput
                        {
                            Contains = filterParams.SearchTerm
                        }
                    }
                ]
            });
        }

        return new VacancyEntityFilterInput
        {
            And = andFilters
        };
    }
}