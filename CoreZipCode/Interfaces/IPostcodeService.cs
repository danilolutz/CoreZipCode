using CoreZipCode.Result;
using System.Threading.Tasks;

namespace CoreZipCode.Interfaces
{
    public interface IPostcodeService
    {
        Task<Result<T>> GetPostcodeAsync<T>(string postcode) where T : class;
    }
}
