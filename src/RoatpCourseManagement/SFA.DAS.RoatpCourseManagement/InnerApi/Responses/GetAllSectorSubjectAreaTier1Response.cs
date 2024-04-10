using System;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public record GetAllSectorSubjectAreaTier1Response(List<SectorSubjectAreaTier1Model> SectorSubjectAreaTier1s);

public record SectorSubjectAreaTier1Model(int SectorSubjectAreaTier1, string SectorSubjectAreaTier1Desc, DateTime EffectiveFrom, DateTime? EffectiveTo);
