using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Extensions
{
    public static class TaskEx
    {
        public static async Task<(T1, T2)> AwaitAll<T1, T2>(Task<T1> t1, Task<T2> t2)
        {
            await Task.WhenAll(t1, t2);
            return (t1.Result, t2.Result);
        }
    }
}
