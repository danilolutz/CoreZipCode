using CoreZipCode.Result;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    public interface IApiHandler
    {
        Task<Result<string>> CallApiAsync(string url);
    }
}
