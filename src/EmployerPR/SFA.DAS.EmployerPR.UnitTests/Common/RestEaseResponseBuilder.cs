using System.Net;
using RestEase;

namespace SFA.DAS.EmployerPR.UnitTests.Common;

public static class RestEaseResponseBuilder
{
    public static Response<T?> GetOkResponse<T>(T value) where T : class
        => new(null, new(HttpStatusCode.OK), () => value);

    public static Response<T?> GetNotFoundResponse<T>() where T : class
        => new(null, new(HttpStatusCode.NotFound), () => default);

}
