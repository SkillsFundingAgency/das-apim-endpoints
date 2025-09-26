using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetVacancyReviewsByFilterRequest(List<string>? status = null, DateTime? expiredAssignationDateTime = null) : IGetApiRequest
{
    public string GetUrl => status != null 
        ? $"api/vacancyreviews?reviewStatus={string.Join("&reviewStatus=", status)}&expiredAssignationDateTime={expiredAssignationDateTime}"
        : $"api/vacancyreviews?reviewStatus=&expiredAssignationDateTime={expiredAssignationDateTime}";
}