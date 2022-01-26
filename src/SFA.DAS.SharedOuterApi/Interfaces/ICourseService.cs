using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICourseService
    {
        List<string> MapRoutesToCategories(IReadOnlyList<string> routes);
    }
}