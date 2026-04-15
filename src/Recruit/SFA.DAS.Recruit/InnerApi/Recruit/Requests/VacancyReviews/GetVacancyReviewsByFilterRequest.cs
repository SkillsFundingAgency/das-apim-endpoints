using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;

public class GetVacancyReviewsByFilterRequest(List<string>? status = null, DateTime? expiredAssignationDateTime = null) : IGetApiRequest
{
    public string GetUrl => status != null 
        ? $"api/vacancyreviews?reviewStatus={string.Join("&reviewStatus=", status)}&expiredAssignationDateTime={expiredAssignationDateTime:yyyy-MMM-dd HH:mm:ss}"
        : $"api/vacancyreviews?reviewStatus=&expiredAssignationDateTime={expiredAssignationDateTime:yyyy-MMM-dd HH:mm:ss}";
}