﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
public class GetAllApprenticeshipsForAcademicYear(string ukprn, int academicyear, int page, int? pageSize = 20) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}";
}

