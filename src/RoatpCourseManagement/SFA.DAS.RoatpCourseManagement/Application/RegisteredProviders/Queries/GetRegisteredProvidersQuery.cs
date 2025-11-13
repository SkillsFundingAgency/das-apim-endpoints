using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;

public record GetRegisteredProvidersQuery : IRequest<ApiResponse<RegisteredProviderResponse>>;
