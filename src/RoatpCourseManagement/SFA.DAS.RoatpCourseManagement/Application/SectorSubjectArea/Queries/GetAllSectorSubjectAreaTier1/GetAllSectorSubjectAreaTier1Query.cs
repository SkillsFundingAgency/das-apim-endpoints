using MediatR;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public record GetAllSectorSubjectAreaTier1Query() : IRequest<ApiResponse<GetAllSectorSubjectAreaTier1Response>>;
