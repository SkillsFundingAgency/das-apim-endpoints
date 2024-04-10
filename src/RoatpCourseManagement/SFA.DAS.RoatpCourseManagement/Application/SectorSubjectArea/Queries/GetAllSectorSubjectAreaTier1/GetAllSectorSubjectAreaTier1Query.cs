using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public record GetAllSectorSubjectAreaTier1Query() : IRequest<ApiResponse<GetAllSectorSubjectAreaTier1Response>>;
